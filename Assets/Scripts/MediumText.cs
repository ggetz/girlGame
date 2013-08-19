using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MediumText : FLabel {
	
	bool solid=true;
	
	/*---------------------------
	 * Variables for twinkling
	 *--------------------------*/
	bool visible;
	int fadeTime;
	int fade;
	int showTime;
	int show;
	int time;
	bool isTwinkling;
	bool showing;
	
	List <MediumText> textObjects = new List<MediumText>(); 
	public MediumText(string font, string text):base(font, text)
	{
		
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	public void Update () {
		
		if(time>0)
		{
			if(showing)
				{
					if(alpha <= 0)
					{
						alpha=100;
						solid=true;
						Debug.Log ("See");
					}
					if(show>0)
					{
						show--;
					}
					else
					{
						show=showTime;
						showing=false;
						time--;
					}
				}
		
			if(!showing)
			{
				if(alpha>0)
				{
					alpha=0;
					solid=false;
					Debug.Log ("Don't See");
				}
				
				if(fade>0)
				{
					fade--;
				}
				else
				{
					fade=fadeTime;
					showing=true;
				}
			}
		}
		
		if(time<=0)
		{
			alpha=0;
			solid=false;
			isTwinkling=false;
		}
	
	}
	
	public bool isSolid()
	{
		return solid;
	}
	
	public void setSolidity(bool isSolid)
	{
		solid=isSolid;
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
		textObjects.Add (obs);
	}
	
	public void twinkle(int showInterval, int fadeInterval, bool visible, int twinkleTime)
	{
		
		if(!isTwinkling)
		{
			showTime = showInterval;
			fadeTime = fadeInterval;
			fade = fadeInterval;
			show = showInterval;
			isTwinkling=true;
			time=twinkleTime;
			showing = visible;
		}
			
	}
}
