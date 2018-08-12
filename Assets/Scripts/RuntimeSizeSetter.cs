using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeSizeSetter : MonoBehaviour {
	RectTransform rect;
	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
		//Debug.Log(Screen.width +","+Screen.height);
		rect.sizeDelta = new Vector2(rect.sizeDelta.x-32, rect.sizeDelta.y-32);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(rect.sizeDelta);
		
	}
}
