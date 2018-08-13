using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonShake : MonoBehaviour {

	Vector3 canonPosition;
	float timer = 0;
	float shakeTime = 0.75f;
	bool shaking = false;

	public void Shake()
	{
		shaking = true;
		timer = Time.time;
	}

	// Use this for initialization
	void Start () {
		canonPosition = transform.position;
		Button b = GetComponent<Button>();
		b.onClick.AddListener(Shake);
	}
	
	// Update is called once per frame
	void Update () {
		if (shaking)
		{
			if (Time.time - timer > shakeTime)
			{
				shaking = false;
				transform.position = canonPosition;
			}
			else
			{
				float normalTime = (Time.time - timer) / shakeTime;
				Vector3 shakeDisplacement = new Vector3((1.0f-normalTime)*(normalTime*Mathf.Sin(15*normalTime)*40),0,0);
				transform.position = canonPosition + shakeDisplacement;
			}
		}
	}
}
