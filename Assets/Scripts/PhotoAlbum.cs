using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PhotoAlbum : MonoBehaviour {

	public Sprite[] sprites1;
	public Sprite[] sprites2;
	public Sprite[] sprites3;

	public int albumIndex;
	public int imageIndex;
	public Vector2 maxSize = new Vector2(600, 950);

	private GameObject[] imageObjects;
	private GameObject imageCarrier;
	public GameObject picOwner;

	private float timer;
	private float timerWait = 0.5f;
	private bool timerOn;

	public UnityAction callBack;
	

	// Use this for initialization
	void Start () {
		imageCarrier = (GameObject)Resources.Load("ImageCarrier");
		if (imageCarrier == null)
		{ throw new System.Exception("ImageCarrier failed to load from resources.");}

		LoadPictures(0);
	}

	/* just in case someone ever sees this and connects this to me, it 
	was written at 5:22am in the morning during a game jam. */
	private Sprite[] Album(int i)
	{
		if (i == 0)
		{return sprites1;}
		else if (i == 1)
		{return sprites2;}
		else if (i == 2)
		{return sprites3;}
		else 
		throw new System.Exception("request for non-existent photoalbum");
	}

	public void LoadPictures(int albumIndex)
	{	
		if (imageObjects != null)
		{
			foreach (GameObject go in imageObjects)
			{Destroy(go);}
		}
		
		picOwner.transform.position = transform.position;

		this.albumIndex = albumIndex;
		imageIndex = 0;
		imageObjects = new GameObject[Album(albumIndex).Length];
		for (int i = 0; i < Album(albumIndex).Length; i++)
		{
			Vector3 displace = new Vector3(maxSize.x, 0, 0);

			imageObjects[i] = 
				GameObject.Instantiate(imageCarrier,transform.position + (i*displace), transform.rotation, 
				picOwner.transform);
			Image image = imageObjects[i].GetComponent<Image>();
			image.sprite = Album(albumIndex)[i];
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
		imageIndex = Mathf.Min(Album(albumIndex).Length-1, imageIndex);

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
	}
	
	// Update is called once per frame
	void Update () {
		
		if (timerOn)
		{
			if (Time.time - timer > timerWait)
			{
				timerOn = false;
				if (imageIndex == imageObjects.Length-1)
				{
					if (callBack != null) { callBack.Invoke();}
				}
			}
			else
			{
				float ratio = (Time.time - timer)/timerWait;
				Vector3 targetLocation = transform.position - new Vector3(maxSize.x * imageIndex, 0, 0);
				picOwner.transform.position = Vector3.Lerp(picOwner.transform.position, targetLocation, ratio);
				
			}
		}
	}
}
