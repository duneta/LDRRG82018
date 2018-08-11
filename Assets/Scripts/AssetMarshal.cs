using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetMarshal : MonoBehaviour {

	[Header("Backgrounds")]
	public Sprite[] backSprites;
	public string[] backNames;

	[Header("Character")]
	public Sprite[] charSprites;
	public string[] charNames;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
		throw new System.Exception("AssetMarshal recieved a request for background:"
			+name+" but asset could not be found.");
	}

	public Sprite Character(string name, string emote)
	{
		emote = emote ?? "happy";
		string descriptor = name + "|" + emote;
		for (int i = 0; i < charNames.Length; i++)
		{
			string candidate = charNames[i];
			if (candidate == descriptor)
			{
				return charSprites[i];
			}
		}
		throw new System.Exception("AssetMarshal recieved a request for character:"
			+descriptor+" but asset could not be found.");
	}
}
