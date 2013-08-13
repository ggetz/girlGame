using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MediumText : FLabel {
	
	bool solid=true;
	List <MediumText> textObjects = new List<MediumText>(); 
	public MediumText(string font, string text):base(font, text)
	{
		
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool isSolid()
	{
		return solid;
	}
	
	public void rotate(float degrees)
	{
		rotation+=degrees;
		
		foreach(MediumText text in textObjects)
		{
			if(textRect.CheckIntersect (text.textRect))
			{
				rotation-=degrees;
			}
		}
	}
	
	public void addCollisionObjects(MediumText obs)
	{
		
	}
}
