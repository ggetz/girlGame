using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterGlass : MovingPictureObstacles
{
	float wscale;
	public Rect glassRect;
	public float height;
	public float width;
	float groundHeight;
	float gravity = 0.4f;
	bool electrified = false;
	int state=1;
	bool shattered;
	List <MovingPictureObstacles> checkHit = new List<MovingPictureObstacles>();
	
	int trembleUp = 5;
	int coolDownTime=15;
	bool isTrembling;
	
	public WaterGlass(string atlas, float scale, float ground): base(atlas)
	{
		state = 0;
		wscale = scale;
		groundHeight=ground;
	}
	// Use this for initialization
	public void Start () 
	{
		height =600*wscale;
		width = 500*wscale;
		glassRect = new Rect(x-width/2f, y+height, width, height);
		Play ("Still", false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isTrembling && coolDownTime > 0)
		{
			coolDownTime--;
		}
		else if(isTrembling && coolDownTime==0)
		{
			trembleUp--;
			coolDownTime=15;
		}
		
		if(isTrembling && trembleUp==0 && state>0)
		{
			state--;
			coolDownTime = 15;
		}
		if(isTrembling && trembleUp==0 && state==0)
		{
			isTrembling=false;
			coolDownTime=15;
		}
	}
	
	public void tremble(int state)
	{
		if(state==1)
		{
			Play ("Tremble1");
		}
		if(state==2)
		{
			Play ("Tremble2");
		}
	}
	
	public void fall()
	{
		Play ("Fall", false);
		while(y > groundHeight)
		{
			y-=gravity;
		}
		
		Stop ();
		Play ("Shatter", false);
		height = 250*wscale;
		width = 1375*wscale;
	}
	
	public bool getElectric()
	{
		return electrified;
	}
	
	public override void action()
	{
		trembleUp+=1;
		
		if(!isTrembling)
		{
			isTrembling=true;
		}
		if(state<3)
		{
			tremble (state);
		}
		
		else
		{
			fall ();
		}
	}
	
	public override Rect getRect()
	{
		return glassRect;
	}
	
	public override void electrify()
	{
		foreach(MovingPictureObstacles obs in checkHit)
		{
			if(glassRect.CheckIntersect (obs.getRect ()))
			{
				obs.electrify();
			}
		}
	}
	
		
	public void addCollider(MovingPictureObstacles obs)
	{
		checkHit.Add (obs);
	}
}
