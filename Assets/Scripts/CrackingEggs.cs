using UnityEngine;
using System.Collections;

public class CrackingEggs : MovingPictureObstacles
{
	int state;
	float escale;
	public Rectangle eggRect;
	public float height;
	public float width;
	Girl girl;

	public CrackingEggs(string atlas, float scale, Girl g): base(atlas)
	{
		state = 0;
		escale = scale;
		girl=g;
		height = 700*0.6f*escale;
		width = 450*0.6f*escale;
	}
	// Use this for initialization
	public override void Start () 
	{
		eggRect = new Rectangle(x-width/2f, y, width, height);
		Debug.Log ("Egg: " + eggRect.top());
		Play ("Uncracked");
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		crack ();
	}
	
	public override void action()
	{
		if(state==0)
		{
			state+=1;
			Play ("Cracked1", false);
		}
		else if(state==1)
		{
			state+=1;
			Play ("Cracked2", false);
		}
		else if(state==2)
		{
			state+=1;
			Play ("Cracked3",false);
			height = 300*escale*0.6f;
			width = 500*escale*0.6f;
			eggRect.height=height;
			eggRect.width=width;
			
		}
		else
		{
			
		}
		
	}
	
	public void crack()
	{
		if(girl.checkCollisions (eggRect) && eggRect.top() >= (girl.getRect().bottom ()-10) && girl.getYVelocity()!=0 )
		{
			Debug.Log ("Let's get crackin'");
			if(state==0)
			{
				
				Play ("Cracked1", false);
			}
			else if(state==1)
			{
				Play ("Cracked2", false);
			}
			else if(state==2)
			{
				Debug.Log ("Egg Original: " + eggRect.top () + " " + eggRect.height);
				Play ("Cracked3",false);
				height = 300*escale*0.6f;
				width = 500*escale*0.6f;
				eggRect.height=height;
				eggRect.width=width;
				Debug.Log ("Egg end: " + eggRect.top () + " " + eggRect.height);
			}
			else
			{
			}
			state++;
		}
	}
	
	public override  Rectangle getRect()
	{
		return eggRect;
	}
}
