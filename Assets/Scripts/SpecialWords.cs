using UnityEngine;
using System.Collections;

public class SpecialWords : MediumText
{
	PictureObstacle effectedObstacle;
	bool linked = false;
	Girl girl;
	
	enum effectTypes{SHRINK, ENLARGE};
	
	// Use this for initialization
	public SpecialWords(string font, string word): base(font, word)
	{
		
	}
	
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public virtual void action()
	{
	}
	
	public bool contactMade()
	{
		return textRect.CheckIntersect(girl.getGirlRect ());
	}
	

	
}
