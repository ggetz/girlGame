using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class Ground
{
	float groundHeight;
	string gFont;
	string textSource;
	
	MediumText groundText;
	
	int level;
	int breaks;
	
	int upperBreakRange;
	int lowerBreakRange;
	
	float upperDistanceRange;
	float lowerDistanceRange;
	
	string text="";
	string tempText;
	
	float textPosition=0;
	float distance=0;
	
	List<Rect> groundObjects = new List<Rect>();
	
	public Ground(float ground, string font, string source, int difficulty)
	{
		groundHeight = ground;
		gFont = font;
		textSource=source;
		level=difficulty;
	}

	// Use this for initialization
	public void Start () 
	{
		try
        {
            using (StreamReader sr = new StreamReader(textSource))
            {
				
				if(level<2)
				{
					text = sr.ReadToEnd ();
	                groundText = new MediumText(gFont, text);
					groundText.scale = 0.6f;
					groundText.SetPosition (groundText.textRect.width/2f, groundHeight);
					Futile.stage.AddChild (groundText);
				}
				
				else
				{
					if(level==2)
					{
						lowerBreakRange=20;
						upperBreakRange=40;
						
						lowerDistanceRange=0.1f;
						upperDistanceRange=0.4f;
					}
					
					if(level==3)
					{
						lowerBreakRange=5;
						upperBreakRange=30;
						
						lowerDistanceRange=0.2f;
						upperDistanceRange=1.5f;
					}
					
					while(!sr.EndOfStream)
					{
						breaks= UnityEngine.Random.Range (lowerBreakRange, upperBreakRange);
						while(!sr.EndOfStream && breaks>0)
						{
							
							tempText=Convert.ToString((char)sr.Read());
							if(tempText==" ")
							{
								breaks--;
							}
							text+=tempText;
						}
						
						groundText=new MediumText(gFont, text);
						groundText.scale=0.6f;
						distance=UnityEngine.Random.Range(Futile.screen.width*lowerDistanceRange, Futile.screen.width*upperDistanceRange);
						if(textPosition==0)
						{
							groundText.SetPosition ((groundText.textRect.width/2f)*0.6f, groundHeight);
							distance=0;
						}
						else
						{
							groundText.SetPosition (textPosition+((groundText.textRect.width/2f)*0.6f)+distance, groundHeight);
							
						}
						
						textPosition+=(groundText.textRect.width)*0.6f+distance;
						Futile.stage.AddChild (groundText);
						groundObjects.Add (groundText.textRect);
						text="";
						breaks=0;
					}
				}
				
            }
        }
        catch (Exception e)
        {
            Debug.Log ("ooops");
			Debug.Log (e);
        }
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public List<Rect> getGroundRect()
	{
		return groundObjects;
	}

}
