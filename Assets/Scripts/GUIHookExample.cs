using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHookExample : MonoBehaviour {

	public Text text;

	public string textA;
	public string textB;

	public bool isTextA = true;

	// Use this for initialization
	void Start () {
		if (text == null)
		{ throw new System.Exception("GUIHookExample object "+gameObject.name+" does not have a GUIText object associated."); }

		text.text = textA;
	}
	
	public void ToggleText()
	{
		if (isTextA)
		{ text.text = textB; }
		else 
		{ text.text = textA; }
		isTextA = !isTextA;
	}
}
