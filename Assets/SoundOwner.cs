using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOwner : MonoBehaviour {

	AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
		source.UnPause();
	}

	void OnDisable()
	{
		source.Pause();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
