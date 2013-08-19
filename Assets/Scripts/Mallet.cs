using UnityEngine;
using System.Collections;

public class Mallet : MovingPictureObstacles
{
	float mscale;
	public Rect malletRect;
	public float height;
	public float width;
	bool electrified = false;
	
	public Mallet(string atlas, float scale): base(atlas)
	{
		mscale = scale;
	}
	
	// Use this for initialization
	public void Start () 
	{
		height =700*mscale;
		width = 400*mscale;
		malletRect = new Rect(x-width/2f, y+height, width, height);
		Play ("Still", false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public override void action()
	{

	}
	
	public override Rect getRect()
	{
		return malletRect;
	}
	
	public override void electrify()
	{
		Play ("Fry", false);
		height = 90*mscale;
		width = 400*mscale;
	}
}
