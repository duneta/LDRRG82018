using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SlideBack : MonoBehaviour {
	float velocity = 0;
	float acceleration = -0.1f;
	Scrollbar scrollbar;
	public UnityEvent OnSlideRight;
	// Use this for initialization
	void Start () {
		scrollbar = GetComponent<Scrollbar>();
	}
	public void ResetVelocity () {
			velocity = 0;
	}
	// Update is called once per frame
	void Update () {
		if (!Input.GetMouseButton(0)) {velocity += acceleration;
			scrollbar.value = Mathf.Max(0,scrollbar.value+velocity);
		}

		if (scrollbar.value > 0.999) {
			scrollbar.interactable = false;

			if (OnSlideRight != null) {
				OnSlideRight.Invoke();
			}
		}

		
	}
}
