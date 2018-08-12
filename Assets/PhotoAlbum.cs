using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbum : MonoBehaviour {

	public Sprite[] sprites;

	public int imageIndex;
	public Vector2 maxSize = new Vector2(600, 950);

	private GameObject[] imageObjects;
	private GameObject imageCarrier;
	public GameObject picOwner;

	private float timer;
	private float timerWait = 0.5f;
	private bool timerOn;
	

	// Use this for initialization
	void Start () {
		imageCarrier = (GameObject)Resources.Load("ImageCarrier");
		if (imageCarrier == null)
		{ throw new System.Exception("ImageCarrier failed to load from resources.");}
		imageObjects = new GameObject[sprites.Length];
		for (int i = 0; i < sprites.Length; i++)
		{
			Vector3 displace = new Vector3(maxSize.x, 0, 0);

			imageObjects[i] = 
				GameObject.Instantiate(imageCarrier,transform.position + (i*displace), transform.rotation, 
				picOwner.transform);
			Image image = imageObjects[i].GetComponent<Image>();
			image.sprite = sprites[i];
			Vector2 size = image.sprite.rect.size;

			if (size.x > maxSize.x)
			{ size *= (maxSize.x/size.x); }
			if (size.y > maxSize.y)
			{ size *= (maxSize.y/size.y); }
			image.rectTransform.sizeDelta = size;
			
		}
	}

	public void MoveAlbumRight()
	{ 
		imageIndex += 1;
		imageIndex = Mathf.Min(sprites.Length-1, imageIndex);

		SetAnimation();
	}

	public void MoveAlbumLeft()
	{ 
		imageIndex -= 1;
		imageIndex = Mathf.Max(0, imageIndex);

		SetAnimation();
	}

	void SetAnimation()
	{
		timer = Time.time;
		timerOn = true;
		Debug.Log("THEN");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (timerOn)
		{
			if (Time.time - timer > timerWait)
			{
				timerOn = false;
			}
			else
			{
				float ratio = (Time.time - timer)/timerWait;
				Vector3 targetLocation = transform.position + new Vector3(maxSize.x * imageIndex, 0, 0);
				picOwner.transform.position = Vector3.Lerp(transform.position, targetLocation, ratio);
			}
		}
	}
}
