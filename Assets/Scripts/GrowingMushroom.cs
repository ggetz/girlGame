using UnityEngine;
using System.Collections;

public class GrowingMushroom : MovingPictureObstacles
{
	string start;
	float mscale;
	Rectangle mushRect;
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
		
		sheight = 300*mscale;
		swidth = 250*mscale;
		bheight = 450*mscale;
		bwidth = 250*mscale;
		
		if(start == "small")
		{
			height = sheight;
			width = swidth;
		}
		
		if(start == "big")
		{
			height = bheight;
			width = bwidth;
			isBig = true;
		}
	}
	// Use this for initialization
	public override void Start () 
	{
		
		if(start == "small")
		{
			mushRect = new Rectangle(x-swidth/2f, y, swidth, sheight);
			Play ("Small");
		}
		
		if(start == "big")
		{
			mushRect = new Rectangle(x-bwidth/2f, y, bwidth, bheight);
			Play ("Big");
			
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
	
	public override Rectangle getRect()
	{
		Debug.Log ("Mushroom: " + mushRect.x);
		return mushRect;
	}
}
