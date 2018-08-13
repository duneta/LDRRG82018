using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameChain : MonoBehaviour {

	public NovelRunner runner;

	public GameObject NovelDisplay;

	public Phone phone;

	public PhotoAlbum album;

	public GameObject photoSet2;
	public GameObject photoSet3;

	public Vector3 phoneOnScreenPosition;
	public Vector3 phoneCenterScreen;
	public Vector3 phoneOffScreenPosition;

	 int guardCounter = 0;
	 public bool GuardClause(int level)
	 {
		if (guardCounter > level) {return true;}
		guardCounter = level + 1;
		return false;
	 }
	// Use this for initialization
	void Start () {
		runner.BeginStory("prolog", EndProlog);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EndProlog()
	{
		phone.SlideTo(DoNothing, phoneCenterScreen);
		runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
	}

	 public void StartAlbum1()
	 {
		if (GuardClause(0)){return;}
		runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
		phone.ShowAlbum();
		album.callBack = StartAlbum1Commentary;
	 }

	 public void StartAlbum1Commentary()
	 {
		if (GuardClause(1)){return;}
		 Debug.Log("I need some fine wine, and you, you need to be nicer.");
		phone.SlideTo(DoNothing, phoneOnScreenPosition);
		phone.SetLock(true);
		runner.gameObject.SetActive(true);
		NovelDisplay.gameObject.SetActive(true);
		runner.BeginStory("photoalbumstart", EndAlbum1Comment);
	 }


	 public void EndAlbum1Comment()
	 {
		if (GuardClause(2)){return;}
		 phone.SlideTo(()=>{
			 runner.BeginStory("janeatrestaurant",EndRestaurant);
		 }, phoneOffScreenPosition);
	 }

	 public void EndRestaurant()
	 {
		if (GuardClause(3)){return;}
		 runner.BeginStory("restaurantphoto2", 
		 ()=>{
			 phone.SlideTo(StartAlbum2, phoneOnScreenPosition);
		 });
	 }

	 public void StartAlbum2()
	 {
		if (GuardClause(4)){return;}
		runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
		phone.SetLock(false);

		 album.LoadPictures(1);
		 phone.ShowAlbum();
		 album.callBack = StartAlbum2Commentary;
	 }

	 public void StartAlbum2Commentary()
	 {
		if (GuardClause(5)){return;}
		phone.SetLock(true);
		runner.gameObject.SetActive(true);
		NovelDisplay.gameObject.SetActive(true);
		 runner.BeginStory("janeCamping", StartCamping);
	 }

	 public void StartCamping()
	 {
		if (GuardClause(6)){return;}
		 phone.SlideTo(()=>{
			 runner.BeginStory("janecampflashback",
			 ()=>
			 {phone.SlideTo(Album3, phoneOnScreenPosition);}
			 );
		 }, phoneOffScreenPosition);
	 }
	
	public void Album3()
	{
		if (GuardClause(7)){return;}
		runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
		phone.SetLock(false);

		 album.LoadPictures(2);
		 phone.ShowAlbum();
		 album.callBack = StartAlbum3Commentary;
	}

	public void StartAlbum3Commentary()
	{
		if (GuardClause(8)){return;}
		phone.SetLock(true);
		runner.gameObject.SetActive(true);
		NovelDisplay.gameObject.SetActive(true);
		 runner.BeginStory("finalalbum", CollegeFlash);
	}

	public void CollegeFlash()
	{
		if (GuardClause(9)){return;}
		 phone.SlideTo(()=>{
			 runner.BeginStory("collegeAcceptance",
			 ()=>{phone.SlideTo(Gobacktoapps, phoneOnScreenPosition); });
		 }, 
		 phoneOffScreenPosition);
	}

	public void Gobacktoapps()
	{
		if (GuardClause(10)){return;}
		//runner.gameObject.SetActive(false);
		NovelDisplay.gameObject.SetActive(false);
		phone.SetLock(true);
		phone.ShowApps();
		runner.BeginStory("texting", End);
	}

	public void End()
	{
		if (GuardClause(11)){return;}
		Debug.Log("The eNd");
	}
	public void DoNothing()
	{}
}
