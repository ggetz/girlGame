using UnityEngine;
using System.Collections;

public class AffectPictureWords : SpecialWords {
	
	MovingPictureObstacles effectedObstacle;
	MediumText effectedText;
	int actionType;
	bool linked = false;
	
	public AffectPictureWords(string font, string word, MovingPictureObstacles obs): base(font, word)
	{
		effectedObstacle = obs;	
		linked = true;
	}
	
	public AffectPictureWords(string font, string word, MediumText obs, int type): base(font, word)
	{
		effectedText = obs;
		actionType = type;
		linked=true;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void action()
	{
		if(linked)
		{
			if(effectedObstacle!=null)
			{
				effectedObstacle.action();
			}
			
			if(effectedText != null)
			{
				if(actionType==1)
				{
					effectedText.rotate(5f);
				}
			}
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
