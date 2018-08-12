using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
	{}
	
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
	}

	public Sprite Background(string name)
	{
		for (int i = 0; i < backNames.Length; i++)
		{
			string candidate = backNames[i];
			if (candidate == name)
			{
				return backSprites[i];
			}
		}
		if (!unfoundItems.Contains(name))
		{ unfoundItems.Add(name);}
		return null;
	}

	public Sprite Character(string name, string emote)
	{
		emote = emote ?? "Happy";
		string descriptor = name + "|" + emote;
		for (int i = 0; i < charNames.Length; i++)
		{
			string candidate = charNames[i];
			if (candidate == descriptor)
			{
				return charSprites[i];
			}
		}
		if (!unfoundItems.Contains(descriptor))
		{ unfoundItems.Add(descriptor);}
   		//throw new System.Exception("AssetMarshal recieved a request for character:"
		//	+descriptor+" but asset could not be found.");
		return null;
	}
}
