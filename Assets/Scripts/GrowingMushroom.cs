using UnityEngine;
using System.Collections;

public class GrowingMushroom : MovingPictureObstacles
{
	string start;
	float mscale;
	Rect mushRect;
	public float sheight;
	public float swidth;
	public float bheight;
	public float bwidth;
	public float height;
	public float width;
	bool isBig = false;
	bool isSold=true;

	public GrowingMushroom(string atlas, string startState, float scale): base(atlas)
	{
		start = startState;
		mscale = scale;
	}
	// Use this for initialization
	public void Start () 
	{
		sheight = 300*mscale;
		swidth = 250*mscale;
		bheight = 500*mscale;
		bwidth = 250*mscale;
		
		if(start == "small")
		{
			height = sheight;
			width = swidth;
			mushRect = new Rect(x, y, swidth, sheight);
			Play ("Small");
		}
		
		if(start == "big")
		{
			height = bheight;
			width = bwidth;
			mushRect = new Rect(x, y, bwidth, bheight);
			Play ("Big");
			isBig = true;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public override void action()
	{
		if(!isBig)
		{
			Play("Grow", false);
			mushRect.height = bheight;
			mushRect.width = bwidth;
			height = bheight;
			width = bwidth;
			isBig=true;
		}
		
		else
		{
			Play ("Shrink", false);
			mushRect.height = sheight;
			mushRect.width = swidth;
			height = sheight;
			width = swidth;
			isBig = false;
		}
	}
}
