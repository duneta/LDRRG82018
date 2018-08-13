using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Phone : MonoBehaviour {

	public float time;
	public float duration;
	public bool timerOn;

	delegate void TimerUpdate(float normal);
	private TimerUpdate update;
	private UnityAction final;

	private Vector3 restingPoint;

	public GameObject appScreen;
	
	public GameObject textApp;
	public GameObject album;
	public GameObject locker;
	public GameObject textOwner;
	private GameObject textSample;

	public Vector2 offset;

	public float weirdScale = 0.5f;
	
	void TimerStart(float duration, TimerUpdate update, UnityAction final)
	{
		time = Time.time;
		timerOn = true;
		this.duration = duration;
		this.update = update;
		this.final = final;
	}

	// Use this for initialization
	void Start () {
		restingPoint = new Vector3(Screen.width*(0.75f), Screen.height/2.0f, 0);
		textSample = (GameObject)Resources.Load("TextSample");
	}
	
	// Update is called once per frame
	void Update () {
		if (timerOn)
		{
			if (Time.time - time > duration)
			{
				timerOn = false;
				if (final!=null){final.Invoke();}
			}
			else
			{
				if (update!=null){ update.Invoke((Time.time-time)/duration);}
			}
		}

		float totalHeight = 0;
		foreach (RectTransform r in GetComponentsInChildren<RectTransform>())
		{
			if (r != this.GetComponent<RectTransform>() && r.GetComponent<TextMessage>() != null)
			{
				totalHeight += r.sizeDelta.y + 8;
			}
		}

		textOwner.GetComponent<RectTransform>().position 
			= //textOwner.transform.parent.GetComponent<RectTransform>().position 
			  (Vector3) offset + new Vector3(0, totalHeight*(weirdScale) ,0);
	}

	public void SlideTo(UnityAction final, Vector3 target)
	{
		restingPoint = target;
		TimerStart(0.5f, 
		(float n) => {
			transform.position = Vector3.Lerp(transform.position, restingPoint, n);
		}, ()=>{transform.position = restingPoint; final.Invoke();});
	}

	public void ShowAlbum()
	{
		appScreen.SetActive(false);
		textApp.SetActive(false);
		album.SetActive(true);
	}
	public void ShowApps()
	{
		appScreen.SetActive(false);
		textApp.SetActive(true);
		album.SetActive(false);
	}

	public void SetLock(bool state)
	{
		locker.SetActive(state);
	}

	public void PostTextMessage(bool blue, string character, string line)
	{
		GameObject text = Instantiate(textSample, textOwner.transform.position, textOwner.transform.rotation, textOwner.transform);
		TextMessage message = text.GetComponent<TextMessage>();
		message.SetCharacter(character);
		message.SetContent(line);
		message.SetReceieve(blue);

		Update();
		
	}
}
