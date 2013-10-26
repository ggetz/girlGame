using UnityEngine;
using System.Collections;

public class Scroll : MovingPictureObstacles {

	float scale;
	public float rHeight = 90;
	public float width = 450;
	public float fHeight=500;
	public float textHeight=200;
	public float textWidth=300;
	Rectangle textRect;
	
	public Scroll(string atlas, float x):base(atlas)
	{
		scale = x;
			
	}

	// Use this for initialization
	public override void Start () 
	{
		rHeight = rHeight*scale;
		width = width*scale;
		fHeight=fHeight*scale;
		Play ("Still");	
		textRect=new Rectangle(0, 0, 0, 0);
		Debug.Log ("Rect: " + textRect.top () + " " + textRect.left ());	
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void action()
	{
		textHeight=textHeight*scale;
		textWidth=textWidth*scale;
		
		Play("Roll Out", false);
		textRect=new Rectangle(x-width/2f, y+fHeight/2f-textHeight/2f, textWidth, textHeight);
		Debug.Log ("Rect: " + textRect.top () + " " + textRect.left ());	
	}
	
	public override Rectangle getRect ()
	{
		return textRect;
	}
}
