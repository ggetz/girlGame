using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterGlass : MovingPictureObstacles
{
	float wscale;
	Rectangle waterRect;
	public float height;
	public float width;
	float groundHeight;
	float gravity = 0.4f;
	bool electrified = false;
	int state=1;
	bool shattered;
	List <MovingPictureObstacles> checkHit = new List<MovingPictureObstacles>();
	
	int trembleUp = 1;
	int coolDownTime=15;
	bool isTrembling;
	bool isTrembling2;
	
	public WaterGlass(string atlas, float scale, float ground): base(atlas)
	{
		state = 1;
		wscale = scale;
		groundHeight=ground;
	}
	// Use this for initialization
	public override void Start () 
	{
		height =600*wscale;
		width = 500*wscale;
		waterRect=new Rectangle(x, y, width, height);
		Play ("Still", false);
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		if(!shattered)
		{
			if(isTrembling && coolDownTime > 0)
			{
				coolDownTime--;
			}
			else if(isTrembling && coolDownTime==0)
			{
				trembleUp--;
				coolDownTime=10;
			}
			
			if(isTrembling && trembleUp==0 && state>0)
			{
				state--;
				coolDownTime = 10;
			}
			if(isTrembling && trembleUp==0 && state==0)
			{
				isTrembling=false;
				coolDownTime=10;
				Play ("Still");
			}
			
			if(trembleUp>5)
			{
				trembleUp=1;
				state++;
				
			}
		}
		
		if(electrified)
		{
			Play ("Electric", false);
		}
	}
	
	public void tremble(int state)
	{
		if(state==0)
		{
			isTrembling=false;
			Play ("Still");
		}
		if(state==1)
		{
			if(!isTrembling)
			{
				isTrembling=true;
				Play ("Tremble1");
			}
			
		}
		if(state==2)
		{
			if(!isTrembling2)
			{
				isTrembling2=true;
				Play ("Tremble2");
			}
		}
		trembleUp++;
		coolDownTime=10;
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
		width = 1400*wscale;
		
		waterRect = new Rectangle(x-width/2f, y, width, height);

	}
	
	public bool getElectric()
	{
		return electrified;
	}
	
	public override void action()
	{
		if(!shattered)
		{
			trembleUp+=1;
			
			if(state==0)
			{
				state=1;
			}
			if(state<3)
			{
				tremble (state);
			}
			
			else
			{
				shattered=true;
				fall ();
			}
		}
	}
	
	public override Rectangle getRect()
	{
		return waterRect;
	}
	
	public override void electrify()
	{
		Play ("Electric", false);
		
		electrified=true;
		foreach(MovingPictureObstacles obs in checkHit)
		{
			if(waterRect.isIntersecting (obs.getRect()))
			{
				obs.electrify();
			}
		}
		
		electrified=false;
	}
	
		
	public void addCollider(MovingPictureObstacles obs)
	{
		checkHit.Add (obs);
	}
}
