using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LockscreenSlide : MonoBehaviour {
public Animator LockscreenSlideAnimation; 
public Scrollbar scrollbar;
public SlideBack slideback;
public LockscreenSlide DeactivateLockscreen;

public
	// Use this for initialization
	void Start () {
		LockscreenSlideAnimation = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SlideAway() {
		LockscreenSlideAnimation.SetTrigger("SlideToRight");
	}

	public void TurnOff() {
		transform.parent.gameObject.SetActive(false);
	}
}
