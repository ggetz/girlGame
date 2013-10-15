/*----------------------------------------
 * Girl.cs
 * Gabby Getz, Laura Mo
 * Script for the main character of the 
 * game and all of her actions and states
 *---------------------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;

public class Girl : GSpineSprite 
{
	
	// physics and speed
	public float runSpeed = 5f;
	public float jumpPower = 10f;
	public float gravity = 0.4f;
	float yVel = 0;
	
	// height width
	public float girlWidth;
	public float girlHeight;
	
	// states of action
	public bool isRunning = false;
	public bool isJumping = false;
	public bool isCrawling = false;
	public bool isStanding = true;
	public bool isIdle = false;
	public bool isFacingRight = true;
	public bool isGrounded = true;
	
	Rectangle rect;
	float crawlingHeight;
	float standingHeight;
	float scale;
	
	int life=10;
	
	// height of ground
	float groundHeight;
	
	public Girl(string girlAtlas, float sc) : base(girlAtlas)
	{
		scale=sc;
		girlWidth = 100*sc;
		standingHeight=225*sc;
		crawlingHeight = 150*sc;
		girlHeight=standingHeight;
	}
	
	// Use this for initialization
	public void Start () 
	{
		idle ();
		rect = new Rectangle(x, y, girlWidth, girlHeight);
	}
	
	// Update is called once per frame, checks for collisions and falls
	public void Update () 
	{
		y += yVel;
		if (!isGrounded)
		{
			yVel -= gravity;
			isJumping = true;
		}
		else
		{	
			if (isJumping)
			{
				if(isFacingRight)
				{
					Play("Forward Landing");
					Queue ("Forward Idle");
				}
				if(!isFacingRight)
				{
					Play("Reverse Landing");
					Queue ("Reverse Idle");
					
				}
				
				isIdle=true;
				isJumping = false;	
				isGrounded = true;
				isStanding=true;
				isRunning = false;
				isCrawling = false;
				
			}
			
		}
		
		//update dimensions
		if (isCrawling)
		{
			girlHeight = crawlingHeight;
			rect.height = girlHeight;
			rect = new Rectangle(x, y, girlWidth, girlHeight);
		}
		else 
		{
			girlHeight = standingHeight;
			rect.height = girlHeight;
			rect = new Rectangle(x, y, girlWidth, girlHeight);
		}
		
		//check collisions
		rect.x = x;
		rect.y = y;
	}
	
	// initiates jump animation and sets states
	public void jump(Vector2 delta)
	{
		
		if (isGrounded)
		{
			Stop ();
			yVel = jumpPower;
			
			if(isFacingRight)
			{
				Play("Forward Jump");
			}
			
			if(!isFacingRight)
			{
				
				Play("Reverse Jump");	
			}
			
			isGrounded=false;
			isJumping = true;
			isCrawling = false;
			isStanding = false;
			isRunning = false;
		}
		
	}
	
	// initiates crawl animation and states
	public void crawl()
	{
		if (!isCrawling)
		{
			Stop();
			if(isFacingRight)
			{
				Play("Forward Crawl");
			}
			
			if(!isFacingRight)
			{
				Play("Reverse Crawl");
			}
			isCrawling=true;
			isStanding = false;
		}
	}
	
	// initiates running animation and states
	public void run()
	{
		if(!isRunning)
		{
			Stop();
			if(isFacingRight)
			{
				Play("Forward Run");
			}
			
			if(!isFacingRight)
			{
				Play("Reverse Run");
			}
			isRunning = true;
		}
	}
	
	// handles back and forth movement of sprite
	public void move(float targetX)
	{
		string forward="";
		string reverse="";
		
		if(!isStanding)
		{
			forward = "Forward Crawl";
			reverse = "Reverse Crawl";
			runSpeed = 3f;
			
		}
		
		if(isStanding)
		{
			forward = "Forward Run";
			reverse = "Reverse Run";
			runSpeed = 5f;	
		}
		
		//if it's close enough to the point of touch, it stops running
		if ( (targetX - x) < -10 )
		{
			//run left
			if (isFacingRight)
			{
				Stop ();
				Play (reverse);
				scaleX = -scaleX;
				isFacingRight = false;
			}
			x -= runSpeed;
		}
		else if ( (targetX-x) > 10 )
		{
			//run right
			if (!isFacingRight)
			{
				Stop ();
				Play (forward);
				scaleX = -scaleX;
				isFacingRight = true;
			}
			
			x += runSpeed;
		}
		else
		{
			if(isGrounded)
			{
				idle ();
			}
		}
	}
	
	// handles back and forth jumping movement
	// things got messy when it was placed with the other left/right movements for some reason
	public void jumpMove(float targetX)
	{
		if ( (targetX - x) < -10 )
		{
			//run left
			if (isFacingRight)
			{
				Stop ();
				Play ("Reverse Mid-Jump");
				scaleX = -scaleX;
				isFacingRight = false;
			}
			x -= runSpeed;
		}
		else if ( (targetX-x) > 10 )
		{
			//run right
			if (!isFacingRight)
			{
				Stop ();
				Play ("Forward Mid-Jump");
				scaleX = -scaleX;
				isFacingRight = true;
			}
			
			x += 3;
		}
		else
		{
			if(isFacingRight)
			{
				Play("Forward Mid-Jump");
			}
			else
			{
				Play("Reverse Mid-Jump");
			}
		}
		
	}
	
	// handles idle aniamtion and state
	
	public void idle()
	{
		Stop ();
		
		string forward;
		string reverse;
		
		if(isStanding)
		{
			forward = "Forward Idle";
			reverse = "Reverse Idle";
		}
		else
		{
			forward = "Forward Crawl Idle";
			reverse = "Reverse Crawl Idle";
		}
		
		if(isFacingRight)
		{
			Play (forward);
		}
		else
		{
			Play(reverse);
		}

		isRunning = false;
		isCrawling = false;
		isIdle = true;
	}
	
	// sets groundHeight
	public void setGroundHeight(float height)
	{
		groundHeight = height;
	}
	
	public void checkCollisions(FLabel power1)
	{
		bool onTop = x - girlWidth > power1.x - power1.textRect.width/2 
			&& x + girlWidth < power1.x + power1.textRect.width/2;
		if (y <= power1.y + power1.textRect.height/2 && onTop)
		{
			y = power1.y + power1.textRect.height/2;
			isGrounded = true;
			isJumping = false;
			isStanding = true;
			isCrawling = false;
		}
	}
	
	
	public void checkCollisions(float ground)
	{
		if (rect.y <= ground)
		{
			isGrounded = true;
			y = ground;
			yVel = 0;
			//Debug.Log("On Ground");
		}
	}
	
	public bool checkCollisions(Rectangle r)
	{
		Vector2[] corners = getRect().corners();
		Vector2 vel = new Vector2(runSpeed, yVel);
		foreach(Vector2 v in corners)
		{
			if(r.doesContain(v + vel))
			{
				float t = 1;
				float d1 = -1;
				float d2 = -1;
				
				//Does it pass through the top?
				d1 = (r.top() - v.y) / vel.y;
				d2 = (vel.x * d1) + v.x;
				if(d1 > 0 && d1 < t && d2 > r.left() && d2 < r.right())
				{
					t = d1;
				}
				
				//Does it pass through the right?
				d1 = (r.right() - v.x) / vel.x;
				d2 = (vel.y * d1) + v.y;
				if(d1 > 0 && d1 < t && d2 > r.top() && d2 < r.bottom())
				{
					t = d1;
				}
				
				//Does it pass through the bottom?
				d1 = (r.bottom() - v.y) / vel.y;
				d2 = (vel.x * d1) + v.x;
				if(d1 > 0 && d1 < t && d2 > r.left() && d2 < r.right())
				{
					t = d1;
				}
				
				//Does it pass through the left?
				d1 = (r.left() - v.x) / vel.x;
				d2 = (vel.y * d1) + v.y;
				if(d1 > 0 && d1 < t && d2 > r.top() && d2 < r.bottom())
				{
					t = d1;
				}
				
				if(t == 1)
				{
					return false;
				}
				runSpeed *= t;
				yVel *= t;
				return true;
			}
		}
		return false;
	}
	
	public Rectangle getRect()
	{
		return rect;
	}
	
	public void erased()
	{
		Debug.Log ("Oh god I'm being erased");
		if(isCrawling)
		{
			if(isFacingRight)
			{
				Play ("Forward Crawl Hit", false);
			}
			else
			{
				Play ("Reverse Crawl Hit", false);
			}
		}
		else
		{
			if(isFacingRight)
			{
				Play ("Forward Hit", false);
			}
			else
			{
				Play ("Reverse Hit", false);
			}
		}
		
		if(alpha>0.5f)
		{
			alpha=alpha*0.9f;
		}
		else
		{
			alpha=alpha*0.85f;
		}
		
		life--;
	}
	
	public int getLife()
	{
		return life;
	}
	
	public void collectDoodle(Doodle target)
	{
		target.Collect ();
		
	}
	
}
