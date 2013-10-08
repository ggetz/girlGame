using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AffectPictureWords : SpecialWords 
{
	
	MovingPictureObstacles effectedObstacle;
	MediumText effectedText;
	List <MediumText> effectedTextGroup;
	int actionType;
	bool linked = false;

	public AffectPictureWords(string font, string word, float scale, MovingPictureObstacles obs): base(font, word, scale)
	{
		effectedObstacle = obs;	
		linked = true;
	}
	
	public AffectPictureWords(string font, string word, float scale, MediumText obs, int type): base(font, word, scale)
	{
		effectedText = obs;
		actionType = type;
		linked=true;
	}

	public AffectPictureWords(string font, string word, float scale, List <MediumText> obs, int type): base(font, word, scale)
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
			
			else if(effectedText != null)
			{
				Debug.Log ("one");
				actionOption ();
			}
			
			else if(effectedTextGroup !=null)
			{
				Debug.Log ("multiple");
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
