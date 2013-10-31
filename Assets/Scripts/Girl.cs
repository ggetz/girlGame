/*----------------------------------------
 * Girl.cs
 * Gabby Getz, Laura Mo, Justin Niosi
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
	public float crawlSpeed = 3f;
	public float jumpPower = 12f;
	public float gravity = 0.4f;
	float yVel = 0;
	float xVel = 0;
	
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
	
	int life=11;
	
	// height of ground
	float groundHeight;
	
	List<Doodle> doodleList = new List<Doodle>();
	Doodle hittingDoodle;
	
	public Girl(string girlAtlas, float sc) : base(girlAtlas)
	{
		scale=sc;
		girlWidth = 110*sc;

		standingHeight=170*sc;
		crawlingHeight = 110*sc;

		standingHeight=190*sc;
		crawlingHeight = 90*sc;

		girlHeight=standingHeight;
	}
	
	// Use this for initialization
	public void Start () 
	{
		idle ();
		rect = new Rectangle(x-girlWidth/2f, y, girlWidth, girlHeight);
	}
	
	// Update is called once per frame, checks for collisions and falls
	public void Update (List<Rectangle> collRects) 
	{
		isGrounded = false;
		yVel -= gravity;
		//update dimensions
		if (isCrawling)
		{
			girlHeight = crawlingHeight;
			rect = new Rectangle(x - girlWidth / 2, y, girlWidth, girlHeight);
		}
		else 
		{
			girlHeight = standingHeight;
			rect = new Rectangle(x - girlWidth / 2, y, girlWidth, girlHeight);
		}
		
		foreach (Rectangle r in collRects)
		{
			checkCollisions(r);
		}
		
		checkCollisions(groundHeight);
		checkDoodleCollision();
		if (!isGrounded)
		{
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
			if(yVel < 0)
			{
				yVel = 0;
			}
		}
		
		x += xVel;
		y += yVel;
		
		rect.x += xVel;
		rect.y += yVel;
		
		xVel = 0;
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
			xVel = crawlSpeed;
		}
		
		if(isStanding)
		{
			forward = "Forward Run";
			reverse = "Reverse Run";
			xVel = runSpeed;	
		}
		
		//if it's close enough to the point of touch, it stops running
		if ( (targetX - x) < -10 )
		{
			//run left
			if (isFacingRight)
			{
				Stop ();
				Play (reverse);
				isFacingRight = false;
				scaleX = -scaleX;
			}
			xVel *= -1;
		}
		else if ( (targetX-x) > 10 )
		{
			//run right
			if (!isFacingRight)
			{
				Stop ();
				Play (forward);
				isFacingRight = true;
				scaleX = -scaleX;
			}
		}
		else
		{
			if(isGrounded)
			{
				idle ();
			}
			xVel = 0;
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
				isFacingRight = false;
				scaleX = -scaleX;
			}
			xVel = runSpeed * -1;
		}
		else if ( (targetX-x) > 10 )
		{
			//run right
			if (!isFacingRight)
			{
				Stop ();
				Play ("Forward Mid-Jump");
				isFacingRight = true;
				scaleX = -scaleX;
			}
			xVel = runSpeed;
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
			xVel = 0;
		}
		
	}
	
	// handles idle animation and state
	
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
		if ((yVel <= 0) && (rect.y <= ground))
		{
			isGrounded = true;
			y = ground;
			//Debug.Log("On Ground");
		}
	}
	
	public bool checkCollisions(Rectangle r)
	{
		if (!r.isSolid)
			return false;
		Vector2[] gCorners = getRect().corners();
		Vector2[] rCorners = r.corners();
		Vector2 vel = new Vector2(xVel, yVel);
		Vector2 negVel = vel * -1;
		float tx = 1;
		float ty = 1;
		float d1;
		float d2;
		//if(Mathf.Abs(r.left() - rect.right()) > 300 || Mathf.Abs(rect.left() - r.right()) > 300)
		//{
		//	return false;	
		//}
		
		if(!r.isSolid)
		{
			return false;
		}
		
		foreach(Vector2 v in gCorners)
		{
			if(r.doesContain(v + vel))
			{
				/*Debug.Log("COLLISION TYPE ONE: \n" 
					+ "XGirl: " + v.x + " YGirl: " + v.y + "\n"
					+ "XRect: " + r.x + " YRect: " + r.y + "\n"
					+ "RectWidth: " + r.width + " RectHeight: " + r.height + "\n"
					+ "XVel: " + vel.x + " YVel: " + vel.y);*/
				
				if(vel.x > 0)
				{
					//Does it pass through the left?
					d1 = (r.left() - v.x) / vel.x;
					d2 = (vel.y * d1) + v.y;
					if(d1 >= 0 && d1 < tx && d2 > r.bottom() && d2 < r.top())
					{
						tx = d1;
					}
				}
				else
				{
					//Does it pass through the right?
					d1 = (r.right() - v.x) / vel.x;
					d2 = (vel.y * d1) + v.y;
					if(d1 >= 0 && d1 < tx && d2 > r.bottom() && d2 < r.top())
					{
						tx = d1;
					}
				}
				
				if(vel.y > 0)
				{
					//Does it pass through the bottom?
					d1 = ( r.bottom() - v.y) / vel.y;
					d2 = (vel.x * d1) + v.x;
					if(d1 >= 0 && d1 < ty && d2 > r.left() && d2 < r.right())
					{
						ty = d1;
					}
				}
				else
				{
					//Does it pass through the top?
					d1 = (r.top() - v.y) / vel.y;
					d2 = (vel.x * d1) + v.x;
					if(d1 >= 0 && d1 < ty && d2 > r.left() && d2 < r.right())
					{
						ty = d1;
						isGrounded = true;
					}
				}
			}
		}
	
		
		foreach(Vector2 v in rCorners)
		{
			if(rect.doesContain(v + negVel))
			{
				/*Debug.Log("COLLISION TYPE TWO: \n" 
					+ "XGirl: " + rect.x + " YGirl: " + rect.y + "\n"
					+ "GirlWidth: " + rect.width + " GirlHeight: " + rect.height + "\n"
					+ "XRect: " + v.x + " YRect: " + v.y + "\n" 
					+ "XVel: " + vel.x + " YVel: " + vel.y);*/
				
				if(vel.x > 0)
				{
					//Does it pass through the right?
					d1 = (rect.right() - v.x) / negVel.x;
					d2 = (negVel.y * d1) + v.y;
					//Debug.Log(d1);
					if(d1 >= 0 && d1 < tx && d2 > rect.bottom() && d2 < rect.top())
					{
						tx = d1;
					}
				}
				else
				{
					//Does it pass through the left?
					d1 = (rect.left() - v.x) / negVel.x;
					d2 = (negVel.y * d1) + v.y;
					//Debug.Log(d1);
					if(d1 >= 0 && d1 < tx && d2 > rect.bottom() && d2 < rect.top())
					{
						tx = d1;
					}
				}
				
				if(vel.x > 0)
				{
					//Does it pass through the top?
					d1 = (rect.top() - v.y) / negVel.y;
					d2 = (negVel.x * d1) + v.x;
					//Debug.Log(d1);
					if(d1 >= 0 && d1 < ty && d2 > rect.left() && d2 < rect.right())
					{
						ty = d1;
						
					}
				}
				else
				{
					//Does it pass through the bottom?
					d1 = (rect.bottom() - v.y) / negVel.y;
					d2 = (negVel.x * d1) + v.x;
					//Debug.Log(d1);
					if(d1 >= 0 && d1 < ty && d2 > rect.left() && d2 < rect.right())
					{
						ty = d1;
						isGrounded = true;
					}
				}
			}
		}
	
		if(tx == 1 && ty == 1)
		{
			return false;
		}
		else
		{
			//Debug.Log("tx: " + tx + " ty: " + ty);
		}
		xVel *= tx;
		yVel *= ty;
		return true;
	}
	
	public Rectangle getRect()
	{
		return rect;
	}

	public Doodle getCollidedDoodle()
	{
    	return hittingDoodle;
  	}
	
	public void erased()
	{
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
			alpha=alpha*0.95f;
		}
		else
		{
			alpha=alpha*0.9f;
		}
		
		life--;
	}
	
	public int getLife()
	{
		return life;
	}
	
	public void addDoodle(Doodle target)
	{
		doodleList.Add (target);
	}
	
	public bool checkDoodleCollision()
	{
		foreach(Doodle target in doodleList)
		{
			if(target.doodleRect.isIntersecting (rect))
			{
		       target.Collect();
		       hittingDoodle=target;
		       return true;
		    }
		}
		return false;
	}
	
	public float getYVelocity()
	{
		return yVel;
	}
	
	public void changeSize(float sc)
	{
		rect.scale (sc);
		girlWidth = 110*sc;
		standingHeight=170*sc;
		crawlingHeight = 110*sc;
		girlHeight=standingHeight;
		
		if(sc<0.6f)
		{
			jumpPower=10f;
		}
		else
		{
			jumpPower=12f;
		}
		
		
	}
	
    
}
