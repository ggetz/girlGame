using UnityEngine;
using System.Collections;

public class Trumpet : MovingPictureObstacles {
	
	int blowCount = 0;
	Scroll affectedScroll;
	float scale;
	public float height = 175;
	public float width = 500;
	Rectangle trumpetRect;
	int time=30;
	bool startCountDown=false;
	
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
		trumpetRect=new Rectangle(x, y, 0, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(startCountDown)
		{
			if(time>0)
			{
				time--;
			}
			
			else
			{
				blowCount=0;
				time=30;
			}
		}
	}
	
	public override void action()
	{
		Play ("Blow", false);
		blowCount+=1;
		if(blowCount==1)
		{
			startCountDown=true;
			//PLAY FIRST TRUMPET HERE
		}
		if(blowCount==2)
		{
			//PLAY SECOND TRUMPET HERE
		}
		if(blowCount==3)
		{
			//PLAY THIRD TRUMPET HERE
			affectedScroll.action ();
			startCountDown=false;
		}
	}
	
	public override Rectangle getRect ()
	{
		return trumpetRect;	
	}

}
