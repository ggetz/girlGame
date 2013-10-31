using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightening : MovingPictureObstacles
{
	float lscale;
	public Rectangle flashRect;
	public float height;
	public float width;
	public float lighteningHeight;
	List <MovingPictureObstacles> checkHit = new List<MovingPictureObstacles>();
	
	public Lightening(string atlas, float scale): base(atlas)
	{
		lscale = scale;
		lighteningHeight = 400*lscale;
		
	}
	// Use this for initialization
	public void Start () 
	{
		height =1*lscale;
		width = 1*lscale;
		flashRect = new Rectangle(x-width/2f, y-height, width, height);
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
		flashRect = new Rectangle(x-width/2f, y-height, width, height);
		Debug.Log ("Lightening: " + flashRect.top() + " " + flashRect.bottom());
		foreach( MovingPictureObstacles obs in checkHit)
		{
			Debug.Log ("Obs: " + obs.getRect ().top() + " " + obs.getRect().bottom());
			if(flashRect.isIntersecting(obs.getRect()))
			{
				Debug.Log ("Hey I see something");
				obs.electrify ();
			}
		}
			
		height = 1*lscale;
		width = 1 * lscale;
		flashRect.height=height;
		flashRect.width = width; 
		flashRect = new Rectangle(x-width/2f, y-height, width, height);
	}
	
	public void addCollider(MovingPictureObstacles obs)
	{
		checkHit.Add (obs);
	}
}
