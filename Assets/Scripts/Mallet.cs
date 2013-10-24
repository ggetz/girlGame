using UnityEngine;
using System.Collections;

public class Mallet : MovingPictureObstacles
{
	float mscale;
	public Rectangle malletRect;
	public float height;
	public float width;
	bool electrified = false;
	
	public Mallet(string atlas, float scale): base(atlas)
	{
		mscale=scale;
		height =700*mscale;
		width = 400*mscale;
		Debug.Log ("In the mallet width is: " + width);
		mscale = scale;
	}
	
	// Use this for initialization
	public override void Start () 
	{
		
		Play ("Still", false);
		malletRect = new Rectangle(x-width/2f, y, width, height);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public override void action()
	{

	}
	
	public override Rectangle getRect()
	{
		return malletRect;
	}
	
	public override void electrify()
	{
		Play ("Fry", false);
		height = 80*mscale;
		width = 280*mscale;
		malletRect.height=height;
		malletRect.width=width;
		Debug.Log (malletRect.width + " " + malletRect.height);
		
	}
}
