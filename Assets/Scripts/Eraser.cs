using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eraser: MovingPictureObstacles
{
	bool isPaused;
	
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
	
	Rectangle eraserRect;
	
	bool isRubbing=false;
	bool erasing=false;
	
	bool startMoving=true;
	
	int difficulty;
	int delay;
	
	/**
	 * Constrictor
	 * @param X
	 * @param Y
	 * @param s - sprite
	 */
	public Eraser (string atlas, Girl pc, float sc, int diff): base(atlas)
	{	
		vel = 7;
		angle = 20;
		girl=pc;
		scale=sc;
		height = 200*scale;
		width = 150*scale;
		eraserRect = new Rectangle(x, y-height/2f, width, height);
		difficulty=diff;
		delay = Random.Range (50*difficulty, difficulty*300);
		isPaused = false;
	}
	
	/*-----------------------------
	 * The closer to 0, the more 
	 * less delay time there is
	 * ---------------------------*/
	
	public void setDifficulty(int diff)
	{
		difficulty=diff;
	}
	
	void makeDelay()
	{
		delay = Random.Range (50*difficulty, difficulty*100);
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
	
	public void Pause()
	{
		isPaused = true;
	}
	
	public void Play()
	{
		isPaused = false;
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
		
		dx = girl.x - x + (float)Random.Range (0, 20);
		dy = girl.y - y + (float)Random.Range (0, 20);
		
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
		eraserRect.x=x-width/2f;
		eraserRect.y=y-height/2f;
		eraserAnimation ();
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
		eraserRect.x=x-width/2f;
		eraserRect.y=y-height/2f;
		count++;	
		
		eraserAnimation ();
	}
	
	private void zigZag()
	{
		Debug.Log ("Count: " + zigZagCount);
		if(zigZagCount<10 && doZigZag)
		{
			if (y < 5)
			{
				changed=true;
				zigZagCount++;
			}
			
			else if(y>Futile.screen.height-5)
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
		
		eraserRect.x=x-width/2f;
		eraserRect.y=y-height/2f;
		
		eraserAnimation ();
	}
	
	/**
	 * Updates the position of the eraser
	 * @param delta
	 */
	public void Update ()
	{	
		if (!isPaused)
		{
		if(startMoving)
		{
			
			//If the eraser leaves the screen
			if (x > x+Futile.screen.width/2f || y > Futile.screen.height || x < x-Futile.screen.width/2f || y < 0)
			{
				if(delay>0)
				{
					delay--;
				}
				
				if(delay<=0)
				{
					makeDelay ();
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
					}
				}

			}
		
			checkCollisionsWithGirl ();
			
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
		
		}
	}
	}
	
	void eraserAnimation()
	{
		if(angle > 0)
		{
			
			if(Mathf.Abs (girl.x-x) < width*1.2f && Mathf.Abs (girl.y-y)<height*1.2f)
			{
				if(!isRubbing)
				{
					isRubbing=true;
					width=width*1.2f;
					Play("Down Left Rub");
					vel=2;
				}
			}
			
			if(Mathf.Abs (girl.x-x)>=width*1.2f || Mathf.Abs (girl.y-y)>=height*1.2f)
			{
				Play("Down Left", false);
		
				if(isRubbing)
				{
					width=width/1.2f;
					isRubbing=false;
					vel=origVel;
				}	
			}
			
		}
		
		else if(angle<0 && angle > -Mathf.PI/2f)
		{
			if(Mathf.Abs (girl.x-x) < width*1.2f && Mathf.Abs (girl.y-y)<height*1.2f)
			{
				if(!isRubbing)
				{
					isRubbing=true;
					width=width*1.2f;
					Play("Down Right Rub");
					vel=2;
				}
				
			}
			
			if(Mathf.Abs (girl.x-x)>=width*1.2f || Mathf.Abs (girl.y-y)>=height*1.2f)
			{
				Play("Down Right", false);
				
				if(isRubbing)
				{
					width=width/1.2f;
					isRubbing=false;
					vel=origVel;
				}
			}
		}
		
		else
		{
			if(Mathf.Abs (girl.x-x) < width*1.2f && Mathf.Abs (girl.y-y)<height*1.2f)
			{
				if(!isRubbing)
				{
					isRubbing=true;
					width=width*1.2f;
					
					Play("Up Rub");
					vel=2;
				}
				
			}
			
			if(Mathf.Abs (girl.x-x)>=width*1.5f || Mathf.Abs (girl.y-y)>=height*1.5f)
			{
				
					
				Play("Up", false);
				
				if(isRubbing)
				{
					width=width/1.2f;
					isRubbing=false;
					vel=origVel;
				}
			}
			
		}
	}
	
	void checkCollisionsWithGirl()
	{
		if(eraserRect.isIntersecting(girl.getRect ()))
		{
			Debug.Log ("Erasing time!");
			if(!erasing)
			{
				erasing=true;
				girl.erased ();
			}
		}
		else
		{
			if(erasing)
			{
				erasing=false;
			}
		}
	}
	
	public void startEraser(bool start)
	{
		startMoving=start;
	}
	
	public void Start()
	{
	}

}

