using UnityEngine;
using System.Collections;

public class Trumpet : MovingPictureObstacles {
	
	int blowCount = 0;
	Scroll affectedScroll;
	float scale;
	public float height = 175;
	public float width = 500;
	Rectangle trumpetRect;
	
	public Trumpet(string atlas, float x, Scroll scroll): base(atlas)
	{
		affectedScroll=scroll;
		scale=x;

	}
	// Use this for initialization
	public override void Start () 
	{
		Play ("Still");
		height=height*scale;
		width = width*scale;	
		trumpetRect=new Rectangle(x, y, width, height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void action()
	{
		Play ("Blow", false);
		blowCount+=1;
		if(blowCount==3)
		{
			affectedScroll.action ();
		}
	}
	
	public override Rectangle getRect ()
	{
		
		return trumpetRect;	
	}

}
