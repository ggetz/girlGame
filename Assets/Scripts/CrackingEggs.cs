using UnityEngine;
using System.Collections;

public class CrackingEggs : MovingPictureObstacles
{
	int state;
	float escale;
	public Rect eggRect;
	public float height;
	public float width;

	public CrackingEggs(string atlas, float scale): base(atlas)
	{
		state = 0;
		escale = scale;
	}
	// Use this for initialization
	public void Start () 
	{
		height = 700*escale;
		width = 450*escale;
		eggRect = new Rect(x, y, width, height);
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
}
