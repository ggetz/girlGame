using UnityEngine;
using System.Collections;

public class AffectPictureWords : MediumText {
	
	PictureObstacle effectedObstacle;
	bool linked = false;
	
	public AffectPictureWords(string font, string word, PictureObstacle obs): base(font, word)
	{
		effectedObstacle = obs;	
		linked = true;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void action()
	{
		if(linked)
		{
			effectedObstacle.action();
		}
		if(!linked)
		{
			Debug.Log ("You dun goofed. No obstacle linked");	
		}
	}
	
	public void changeBack()
	{
	}
}
