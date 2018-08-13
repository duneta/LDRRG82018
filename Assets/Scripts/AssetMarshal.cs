using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AssetMarshal : MonoBehaviour {

	[Header("Backgrounds")]
	public Sprite[] backSprites;
	public string[] backNames;

	[Header("Character")]
	public Sprite[] charSprites;
	public string[] charNames;

	[SerializeField]
	private List<string> unfoundItems;

	private UnityAction[] procedures;
	private string[] procedureNames;

	public NovelRunner runner;
	public GameObject novelDisplay;

	private int goodpoints;
	private int badpoints;

	private string lastCharacterExpresssionRequest;

	// Use this for initialization
	void Awake () {
		unfoundItems = new List<string>();
		procedures = new UnityAction[] {
			() => {goodpoints += 1;},
			() => {badpoints += 1;},
			() => {
				runner.gameObject.SetActive(true);
				novelDisplay.gameObject.SetActive(true);

				if (goodpoints > badpoints)
				{
					runner.BeginStory("good ending", (End));
				}
				else
				{
					runner.BeginStory("bad ending", (End));
				} 

			}
		};
		procedureNames = new string[]{
			"GOODPOINT",
			"BADPOINT",
			"finalDispatch"
		};
	}

	private void End()
	{
		SceneManager.LoadScene("Credits");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ExecuteProcedure(string name)
	{
		for (int i = 0; i < procedureNames.Length; i++)
		{
			string candidate = procedureNames[i];
			if (candidate == name)
			{
				procedures[i].Invoke();
				return;
			}
		}
		if (!unfoundItems.Contains(name))
		{ unfoundItems.Add(name);}
		Log();
	}

	public Sprite Background(string name)
	{
		for (int i = 0; i < backNames.Length; i++)
		{
			string candidate = backNames[i];
			if (candidate==name)
			{
				return backSprites[i];
			}
		}
		if (!unfoundItems.Contains(name))
		{ unfoundItems.Add(name);}
		Log();
		return null;
	}

	public Sprite Character(string name, string emote)
	{
		string descriptor = name + "|" + emote;
		if (emote == null) {descriptor = lastCharacterExpresssionRequest;}
		if (emote == null) {descriptor = name + "|";}
		lastCharacterExpresssionRequest = descriptor;
		for (int i = 0; i < charNames.Length; i++)
		{
			string candidate = charNames[i];
			if (candidate.StartsWith(descriptor))
			{
				lastCharacterExpresssionRequest = descriptor;
				return charSprites[i];
			}
		}
		if (!unfoundItems.Contains(descriptor))
		{ unfoundItems.Add(descriptor);}
   		//throw new System.Exception("AssetMarshal recieved a request for character:"
		//	+descriptor+" but asset could not be found.");
		Log();
		return null;
	}

	public void Log()
	{
		string log = string.Empty;
		foreach (string item in unfoundItems)
		{
			log+=item+"\n";
		}
		Debug.Log(log);
	}
}

