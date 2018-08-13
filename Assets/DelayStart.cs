using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DelayStart : MonoBehaviour {

	public UnityEvent action;

	private float timerStart;
	public float timerDuration = 0.5f;
	private bool timerOn = false;
	
	// Use this for initialization
	void Start () {
		timerOn = true;
		timerStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if (timerOn && Time.time - timerStart > timerDuration)
		{
			timerOn = false;
			if (action!=null){action.Invoke();}
		}
	}
}
