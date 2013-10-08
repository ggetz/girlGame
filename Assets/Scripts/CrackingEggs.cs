using UnityEngine;
using System.Collections;

public class CrackingEggs : MovingPictureObstacles
{
	int state;
	float escale;
	public Rect eggRect;
	public float height;
	public float width;
	Girl girl;

	public CrackingEggs(string atlas, float scale, Girl g): base(atlas)
	{
		state = 0;
		escale = scale;
		girl=g;
	}
	// Use this for initialization
	public void Start () 
	{
		height = 700*escale;
		width = 450*escale;
		eggRect = new Rect(x-width/2f, y-height/2f, width, height);
		Play ("Uncracked");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
		}
		else
		{
			height = 300*escale;
			width = 500*escale;
		}
		
	}
	
	public void crack()
	{
		/*if(girl.checkCollisions (eggRect) && girl.getRect().bottom() >= (eggRect.yMax - 10f))
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
			}
			else
			{
				height = 300*escale;
				width = 500*escale;
			}
		}*/
	}
}
