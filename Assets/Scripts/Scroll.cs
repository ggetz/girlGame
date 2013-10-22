using UnityEngine;
using System.Collections;

public class Scroll : MovingPictureObstacles {

	float scale;
	public float height = 90;
	public float width = 450;
	Rectangle textRect;
	
	public Scroll(string atlas, float x):base(atlas)
	{
		scale = x;
			
	}

	// Use this for initialization
	public override void Start () 
	{
		height = height*scale;
		width = width*scale;
		Play ("Still");	
		textRect=new Rectangle(x-width/2f, y+height/2f, width, height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void action()
	{
		Play("Roll Out", false);
		//textRect=new Rectangle(x-width/2f, y
		
	}
	
	public override Rectangle getRect ()
	{
		return textRect;
	}
}
