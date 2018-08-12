using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMessage : MonoBehaviour {
	public Image back; 
	public Text characterText;
	public Text LineText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector2 Size()
	{
		return back.rectTransform.sizeDelta;
	}

	public void SetReceieve(bool val)
	{
		back.color = (val) ? new Color(0.8f,1,0.8f) : new Color(1,1,1);
	}

	public void SetCharacter(string character)
	{
		characterText.text = character;
	}

	public void SetContent(string line)
	{
		LineText.text = line;
	}
}
