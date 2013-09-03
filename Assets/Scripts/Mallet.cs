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
		Play ("Still", false);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public override void action()
	{

	}
	
	public void makeRect()
	{
		malletRect = new Rect(x-width/2f, y, width, height);
	
		Debug.Log ("Mallet: " + y + " " +height + " " + malletRect.xMin + " " + malletRect.xMax + ", " + malletRect.yMin + " " + malletRect.yMax);
	}
	
	public override Rect getRect()
	{
		return malletRect;
	}
	
	public override void electrify()
	{
		Debug.Log ("Mallet frying!");
		Play ("Fry", false);
		height = 90*mscale;
		width = 400*mscale;
	}
}
