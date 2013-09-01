using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eraser: MovingPictureObstacles
{
	/** X and Y coordinates */
	public int count;
	
	public float dx;
	public float dy;
	
	/** Will tell the game to react accordingly when the sprite is hit by the eraser*/
	public bool isHit = false;
	public bool changed = false;
	
	/** Radius of the eraser */
	
	
	/** Maximum speed of the eraser */
	private int maxSpeed = 5;
	
	/** Velocity and angle of eraser swipe */
	private float vel;
	private float angle;
	private int score;
	
	private int strokeType;
	private int pathType;
	private float height;
	private float width;
	private int zigZagCount=0;
	private bool doZigZag;
	Girl girl;
	float origVel;

	
	/**
	 * Constrictor
	 * @param X
	 * @param Y
	 * @param s - sprite
	 */
	public Eraser (string atlas, Girl pc, float sc): base(atlas)
	{	
		vel = 7;
		angle = 20;
		girl=pc;
		scale=sc;
		height = 300*scale;
		width = 200*scale;
	}
	
	public void setScore(int theScore)
	{
		score = theScore;
	}
	
	/**
	 * The code that executes when the eraser is hit
	 */
	private void hit()
	{
		isHit = true;
		
	}
	
	private void randomStroke(int area)
	{
		isHit = false;
		
		vel = Random.Range(0, maxSpeed)+ score/100 + 2;
		origVel=vel;
		
		if(area==0)
		{
			x = (float) Random.Range (girl.x-Futile.screen.width/2.5f, girl.x+Futile.screen.width/2f);
			y = Futile.screen.height;
		}
		
		if(area==1)
		{
			x = 0;
			y = (float)Random.Range (Futile.screen.height/10f, Futile.screen.height);
		}
		
		dx = girl.x - x - 10 + (float)Random.Range (0, 20);
		dy = girl.y - y - 10 + (float)Random.Range (0, 20);
		
		angle = Mathf.Atan(dy/dx);
	}
	

	private void straightPath()
	{
			if(angle<0)
			{
				x += vel * Mathf.Cos(angle);
				y += vel * Mathf.Sin (angle);
			
			}

			if(angle>0)
			{
				x -= vel * Mathf.Cos(angle);
				y -= vel * Mathf.Sin(angle);
			}
		
		
		
	}
	private void trackPath()
	{
		if(angle<0)
		{
			x += vel * Mathf.Cos(angle);
			y += vel * Mathf.Sin(angle);
		
		}
		
		if(angle>0)
		{
			x -= vel * Mathf.Cos(angle);
			y -= vel * Mathf.Sin(angle);
		}
		
		if(count==25)
		{
			
			dx = girl.x - x ;
			dy = girl.y - y;
			angle = Mathf.Atan(dy/dx);
			count=0;
		}
		
		count++;	
	}
	
	private void zigZag()
	{
		if(zigZagCount<10 && doZigZag)
		{
			if (y < 20)
			{
				changed=true;
				zigZagCount++;
			}
			
			else if(y>540)
			{
				changed=false;
				zigZagCount++;
			}
				
			if(changed)
			{
				x += 2*vel * Mathf.Cos(angle);
				y += 2*vel * Mathf.Sin(angle);
			}
			
			if(!changed)
			{
				x += 2*vel * Mathf.Cos(angle);
				y -= 2*vel * Mathf.Sin(angle);
			}	
		}
		else
		{
			if(doZigZag)
			{
				zigZagCount=0;
				doZigZag=false;
			}
			
			x += 2*vel * Mathf.Cos(angle);
			y += 2*vel * Mathf.Sin(angle);
		}
	}
	
	/**
	 * Updates the position of the eraser
	 * @param delta
	 */
	public void Update ()
	{	
		//If the eraser leaves the screen
		if (x > x+Futile.screen.width/2f || y > Futile.screen.height || x < x-Futile.screen.width/2f || y < 0)
		{
			strokeType=Random.Range (0, 1);
			//make a random stroke
			randomStroke(strokeType);
			pathType=Random.Range (0, 5);
			if(pathType==3)
			{
				x=girl.x-girl.girlWidth*2f;
				y=10;
				angle=Mathf.Atan(5);
				doZigZag=true;
				Debug.Log ("angle:" + angle);
			}
		}
	
		if(angle > 0)
		{
			if(Mathf.Abs (girl.x-x) < width*1.5f && Mathf.Abs (girl.y-y)<height*1.5f)
			{
				Play("Down Left Rub");
				vel=1;
			}
			else
			{
				Play("Down Left", false);
				vel=origVel;
			}
		}
		else if(angle<0 && angle > -Mathf.PI/2f)
		{
			if(Mathf.Abs (girl.x-x) < width*1.5f && Mathf.Abs (girl.y-y)<height*1.5f)
			{
				Play("Down Right Rub");
				vel=1;
			}
			else
			{
				Play("Down Right", false);
				vel=origVel;
			}
			
		}
		else
		{
			if(Mathf.Abs (girl.x-x) < width*1.5f && Mathf.Abs (girl.y-y)<height*1.5f)
			{
				Play("Up Rub");
				vel=1;
			}
			else
			{
				Play("Up", false);
				vel=origVel;
			}
			
		}
		
		if(pathType<3)
		{
			straightPath();
		
		}
		
		else if(pathType==3)
		{
			zigZag();
					
		}
		
		
		else if(pathType < 5)
		{
			trackPath();
		}
		
		// Conditions of a collision (x and y axis)
		/*bool collisionX = (X+radius - sprite.x > 0 && X+radius -(sprite.x + sprite.width) < 0);
		bool collisionY = (Y+radius - sprite.y > 0 && Y-radius -(sprite.y + (sprite.height-20)) < 0);
		
		if (collisionX && collisionY)
			hit();*/
	}
	
	public void Start()
	{
		Play("Down Right", false);
	}

}

