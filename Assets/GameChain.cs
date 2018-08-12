﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameChain : MonoBehaviour {

	public NovelRunner runner;

	public GameObject NovelDisplay;

	public Phone phone;

	public PhotoAlbum album;

	public GameObject photoSet2;
	public GameObject photoSet3;

	// Use this for initialization
	void Start () {
		runner.BeginStory("prolog", EndProlog);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EndProlog()
	{
		phone.SlideTo(StartUnlock, new Vector3(Screen.width * (0.75f), Screen.height/2.0f, 0));
		runner.gameObject.SetActive(false);
	}
	 public void StartUnlock()
	 {
		runner.gameObject.SetActive(true);
		runner.BeginStory("unlock", EndUnlock);
	 }

	 public void EndUnlock() 
	 {
		runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
	 }

	 bool guard1 = false;
	 public void StartAlbum1()
	 {
		 if (guard1) {return;}
		 guard1 = true;
		runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
		phone.ShowAlbum();
		album.callBack = StartAlbum1Commentary;
	 }

	 public void StartAlbum1Commentary()
	 {
		phone.SetLock(true);
		runner.gameObject.SetActive(true);
		NovelDisplay.gameObject.SetActive(true);
		runner.BeginStory("photoalbumstart", EndAlbum1Comment);
	 }

	 public void EndAlbum1Comment()
	 {
		 phone.SlideTo(()=>{
			 runner.BeginStory("janeatrestaurant",EndRestaurant);
		 }, 
		 new Vector3(Screen.width * (2.75f), Screen.height/2.0f, 0));
	 }

	 public void EndRestaurant()
	 {
		 runner.BeginStory("restaurantphoto2", 
		 ()=>{
			 phone.SlideTo(StartAlbum2, new Vector3(Screen.width * (0.75f), Screen.height/2.0f, 0));
		 });
	 }

	 public void StartAlbum2()
	 {
		runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
		phone.SetLock(false);

		 album.LoadPictures(1);
		 phone.ShowAlbum();
		 album.callBack = StartAlbum2Commentary;
	 }

	 public void StartAlbum2Commentary()
	 {
		phone.SetLock(true);
		runner.gameObject.SetActive(true);
		NovelDisplay.gameObject.SetActive(true);
		 runner.BeginStory("janeCamping", StartCamping);
	 }

	 public void StartCamping()
	 {

		 phone.SlideTo(()=>{
			 runner.BeginStory("janecampflashback",
			 ()=>
			 {phone.SlideTo(Album3, new Vector3(Screen.width * (0.75f), Screen.height/2.0f, 0));}
			 );
		 }, 
		 new Vector3(Screen.width * (2.75f), Screen.height/2.0f, 0));

	 }
	
	public void Album3()
	{
		runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
		phone.SetLock(false);

		 album.LoadPictures(2);
		 phone.ShowAlbum();
		 album.callBack = StartAlbum3Commentary;
	}

	public void StartAlbum3Commentary()
	{
		phone.SetLock(true);
		runner.gameObject.SetActive(true);
		NovelDisplay.gameObject.SetActive(true);
		 runner.BeginStory("finalalbum", CollegeFlash);
	}

	public void CollegeFlash()
	{
		 phone.SlideTo(()=>{
			 runner.BeginStory("collegeAcceptance",
			 ()=>{phone.SlideTo(Gobacktoapps, new Vector3(Screen.width * (0.75f), Screen.height/2.0f, 0)); });
		 }, 
		 new Vector3(Screen.width * (2.75f), Screen.height/2.0f, 0));
	}

	public void Gobacktoapps()
	{
		//runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
		phone.SetLock(true);
		phone.ShowApps();
		runner.BeginStory("texting", End);
	}

	public void End()
	{
		Debug.Log("The eNd");
	}








	public void DoNothing()
	{}
}