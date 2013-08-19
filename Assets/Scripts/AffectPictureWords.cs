using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AffectPictureWords : SpecialWords {
	
	MovingPictureObstacles effectedObstacle;
	MediumText effectedText;
	List <MediumText> effectedTextGroup;
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

	public AffectPictureWords(string font, string word, List <MediumText> obs, int type): base(font, word)
	{
		effectedTextGroup = obs;
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
				actionOption ();
			}
			
			if(effectedTextGroup.Count>0)
			{
				foreach(MediumText obs in effectedTextGroup)
				{
					effectedText=obs;
					actionOption ();
				}
			}
		}
		if(!linked)
		{
			Debug.Log ("You dun goofed. No obstacle linked");	
		}
	}
	
	public void actionOption()
	{
		if(actionType==1)
		{
			effectedText.rotate(5f);
		}
		if(actionType==2)
		{
			int random = Random.Range (0,1);
			if(random==1)
			{
				effectedText.twinkle (Random.Range (200,300), Random.Range (100,200), true,10);
			}
			
			else
			{
				effectedText.twinkle (Random.Range(200,300), Random.Range (100,200),false,10);
			}
		}
	}
}
