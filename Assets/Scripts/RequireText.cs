using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RequireText : MonoBehaviour {

	public UnityEvent onfinalText;

	Button button;

	public InputField field;

	public string output;
	
	// Use this for initialization
	void Start () {
		button = GetComponent<Button>();
		button.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTextFinal()
	{
		if (onfinalText != null)
		{ onfinalText.Invoke(); }
	}

	public void OnTextEdit(string inbound)
	{
		output = inbound;
		button.interactable = inbound != string.Empty;
	}
}
