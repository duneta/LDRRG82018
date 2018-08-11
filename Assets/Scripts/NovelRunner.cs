using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NovelRunner : MonoBehaviour {

	public enum RunnerStates {idle, waitOnTimer, waitOnInput, waitOnChoice}
	RunnerStates state = RunnerStates.idle;

	public bool running = false;

	public float passiveElementTime = 1.0f;

	public float timer = 0;
	
	public AssetMarshal marshal;

	public Text title;
	public Text message;
	public Image background;
	public GameObject normalDisplay;
	public GameObject optionDisplay;
	public GameObject optionContent;
	public Image characterGraphic;

	public List<NovelBranch> branches;
	
	public string startingBranch;

	private NovelBranch currentBranch;

	private Tokens lastToken;
	private int currentFrame = 0;

	private GameObject providedButton;

	void Start()
	{
		Scrollbar bar = GetComponent<Scrollbar>();
		

		providedButton = (GameObject) Resources.Load("Button");
		if (providedButton == null)
		{ throw new System.Exception("providedButton template could not be loaded from resources."); }

		ParseScript parser = new ParseScript();
		branches = parser.LoadScript();
		lastToken = Tokens.none;
		Init();
		BeginStory();
	}

	public void BeginStory()
	{
		running = true;
		currentFrame = 0;
		Advance(true);
	}
	public void BeginStory(string title)
	{
		startingBranch = title;
		Init();
		BeginStory();
	}

	void Advance(bool doNotIncrement = false)
	{
		lastToken = currentBranch.frames[currentFrame].token;
		if (!doNotIncrement)
		{ currentFrame++; }

		if (currentFrame >= currentBranch.frames.Count)
		{
			running = false;
			return;
		}

		NovelFrame frame = currentBranch.frames[currentFrame];
		switch( frame.token)
		{
			case Tokens.background:
			{ 
				title.text = string.Empty;
				message.text = string.Empty;
				background.sprite = marshal.Background((string)frame.payload);
				StartTimer();
			} break;
			case Tokens.monologue:
			{
				title.text = "monologue";
				message.text = (string)frame.payload;
				state = RunnerStates.waitOnInput;
			} break;
			case Tokens.character:
			{ 
				NovelCharacter character = ((NovelCharacter) frame); 
				if (character.name != "You")
				{ 
					characterGraphic.gameObject.SetActive(true);
					characterGraphic.sprite = marshal.Character(character.name,null);
					characterGraphic.rectTransform.sizeDelta = Scale(Screen.height, 1080, characterGraphic.sprite.rect.size);
				}
				title.text = ((NovelCharacter) frame).name;
				message.text = string.Empty;
				state = RunnerStates.waitOnTimer;
			} break;
			case Tokens.expression:
			{ 
				NovelExpression expression = ((NovelExpression) frame); 
				if (expression.character != "You")
				{ 
					characterGraphic.gameObject.SetActive(true);
					characterGraphic.sprite = marshal.Character(expression.character,(string)expression.payload);
					characterGraphic.rectTransform.sizeDelta = Scale(Screen.height, 1080, characterGraphic.sprite.rect.size);
				}

				title.text = ((NovelExpression)frame).character;
				state = RunnerStates.waitOnTimer;
			} break;
			case Tokens.line:
			{
				if (lastToken == Tokens.character || lastToken == Tokens.expression || lastToken == Tokens.line)
				{ /* Do Nothing */ }
				else
				{ title.text = ((NovelExpression)frame).character + " " + ((NovelExpression)frame).payload; }
				message.text = (string)frame.payload;
				state = RunnerStates.waitOnInput;
			} break;
			case Tokens.hidecharacter:
			{
				title.text = "HIDE CHARACTER";
				message.text = (string) frame.payload;
				StartTimer();
			} break;
			case Tokens.directive:
			{
				title.text = "DIRECTIVE";
				message.text = (string) frame.payload;
				StartTimer();
			} break;
			case Tokens.jump:
			{
				StartTimer();
				Jump((string)frame.payload);
			} break;
			case Tokens.options:
			{
				state = RunnerStates.waitOnChoice;
				NovelOptions options = (NovelOptions) frame;
				
				normalDisplay.SetActive(false);
				optionDisplay.SetActive(true);

				Transform[] old = optionContent.GetComponentsInChildren<Transform>();
				Debug.Log(old.Length);

				foreach (Transform go in old)
				{ GameObject.Destroy(go); }

				foreach (NovelOption option in options.options)
				{
					GameObject g = GameObject.Instantiate(providedButton, optionContent.transform.position,
						optionContent.transform.rotation, optionContent.transform);
					g.GetComponentInChildren<Text>().text = (string)option.payload;
					g.GetComponent<Button>().onClick.AddListener( 
						() => { EndOptions(); Jump(option.target); } /// CLOSURE! to the rescue!!
					);
				}
			} break;
			default:
			{ 
				throw new System.Exception("Unexpected token "+frame.token+" in switch statement.");
			}
		}
	}

	void EndOptions()
	{
		state = RunnerStates.idle;
		normalDisplay.SetActive(true);
		optionDisplay.SetActive(false);
	}

	void StartTimer()
	{
		state = RunnerStates.waitOnTimer;
		timer = Time.time;
	}

	void Update()
	{
		if (running)
		{ 
			if (state == RunnerStates.idle)
			{ Advance(); }
			else if (state == RunnerStates.waitOnInput)
			{
				if (Input.GetButtonDown("Jump"))
				{ Advance(); }
			}
			else if (state == RunnerStates.waitOnTimer)
			{
				if (Time.time - timer > passiveElementTime)
				{ Advance(); }
			}
			else if (state == RunnerStates.waitOnChoice)
			{
				/* Do Nothing, Handled Else Where */
			}
		}
	}

	Vector2 Scale(float actual, float standard, Vector2 patient)
	{
		return patient * (actual/standard);
	}
	void Jump(string title)
	{
		startingBranch = title;
		Init();
		currentFrame = 0;
		Advance(true);
	}

	void Init()
	{
		foreach( NovelBranch branch in branches)
		{
			if (branch.title == startingBranch)
			{
				currentBranch = branch;
				break;
			}
		}

		if (currentBranch == null)
		{ 
			throw new System.Exception("NovelRunner tried to start the novel, "
				+"but it was looking to start with "+startingBranch+" and didn't find it.");
		}
	}
	
}
