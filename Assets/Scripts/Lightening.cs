using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightening : MovingPictureObstacles
{
	float lscale;
	public Rect flashRect;
	public float height;
	public float width;
	List <MovingPictureObstacles> checkHit = new List<MovingPictureObstacles>();
	
	public Lightening(string atlas, float scale): base(atlas)
	{
		lscale = scale;
		
	}
	// Use this for initialization
	public void Start () 
	{
		height =1*lscale;
		width = 1*lscale;
		flashRect = new Rect(x-width/2f, y, width, height);
		Play ("Still", false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public override void action()
	{
		Play("Strike", false);
		height = 500*lscale;
		width = 300 * lscale;
		flashRect.height=height;
		flashRect.width = width; 
		
		foreach( MovingPictureObstacles obs in checkHit)
		{
			if(flashRect.CheckIntersect (obs.getRect ()))
			{
				Debug.Log ("Hey I see something");
				obs.electrify ();
			}
		}
			
		height = 1*lscale;
		width = 1 * lscale;
		flashRect.height=height;
		flashRect.width = width; 
	}
	
	public void addCollider(MovingPictureObstacles obs)
	{
		checkHit.Add (obs);
	}
}
