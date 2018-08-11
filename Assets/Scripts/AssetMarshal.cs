using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetMarshal : MonoBehaviour {

	[Header("Backgrounds")]
	public Sprite[] backSprites;
	public string[] backNames;

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
}
