using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;

// remember to extend FMultiTouchableInterface! (wow that's a long title)
public class VelveteenLevel: MonoBehaviour, FMultiTouchableInterface 
{
	Vector2 deltaSwipe;
	
	Girl girl;
	
	Eraser eraser;
	
	float groundHeight;

	FSprite background;
	Ground ground;
	FSprite obstacle1;
	FLabel power1;
	FSprite introText;
	public float scaleGirl;
	
	bool directionTouchFound;
	bool inSpecialWord=false;
	
	List <SpecialWords> specialWords = new List<SpecialWords> ();
	List <MediumText> mediumText = new List<MediumText>();
	List <PictureObstacle> pictures = new List<PictureObstacle>();
	List <SpecialWords> holdSpecialWords = new List<SpecialWords> ();
	
	FCamObject cam;
	FNode focus;
	
	string blockFont;
	string specialFont;
	
	void Start()
	{
		// Setup Futile
		FutileParams fparams = new FutileParams(true, true, false, false);
		fparams.AddResolutionLevel(1024.0f,	1.0f, 1.0f, "");
		fparams.origin = new Vector2(0f, 0f);
		Futile.instance.Init(fparams);
		
		groundHeight = 0.25f * Futile.screen.halfHeight;
		
		//set up camera
		Futile.stage.AddChild(cam = new FCamObject());
		focus = new FNode();

		//LoadTextures ();
		//SetUpStage ();
		
	}
	
	public void HandleMultiTouch(FTouch[] touches)
	{

		foreach(FTouch touch in touches)
		{
			if(touch.phase == TouchPhase.Began)
			{
				for(int i = specialWords.Count-1; i>=0; i--)
					{
						SpecialWords word = specialWords[i];
						Vector2 touchPos = word.GlobalToLocal (touch.position);
						if(word.textRect.Contains (touchPos) && word.contactMade())
						{
							word.action ();
							inSpecialWord=true;
						}
					}
				
				if(!inSpecialWord)
				{
					if(girl.isIdle)
					{
						if(girl.isStanding)
						{
							girl.run ();
						}
						if(!girl.isStanding)
						{
							girl.crawl ();
						}
					}
					girl.move (touch.position.x - focus.x);
				}
			}
				
				if(touch.phase == TouchPhase.Moved)
				{
					deltaSwipe = touch.deltaPosition;
					if(Mathf.Abs(deltaSwipe.x) > 10)
					{
						if(girl.isIdle)
						{
							girl.isIdle=false;
							
							if(girl.isStanding && girl.isGrounded)
							{
								girl.run ();
							}
							if(!girl.isStanding && girl.isGrounded)
							{
								girl.crawl ();
							}
						}
						
						if(girl.isGrounded)
						{
							if(!girl.isRunning && girl.isStanding)
							{
								girl.run ();
							}
							if(!girl.isStanding && !girl.isCrawling)
							{
								girl.crawl ();
							}
							girl.move (touch.position.x + focus.x);
						}
						if(girl.isJumping)
						{
							girl.jumpMove(touch.position.x + focus.x);
						}
					}
					
					if(deltaSwipe.y > 10)
					{
						
						if(girl.isGrounded && girl.isStanding)
						{
							girl.jump (deltaSwipe);
						}
						
						if(girl.isJumping && !girl.isGrounded)
						{
							girl.jumpMove(touch.position.x + focus.x);
						}
						
						if(!girl.isStanding && girl.isGrounded)
						{
							girl.isStanding = true;
							girl.isGrounded=true;
							girl.isCrawling = false;
							girl.isRunning = false;
							girl.idle ();
						}
						
					}
					
					if(deltaSwipe.y < -10)
					{
						if(girl.isStanding && girl.isGrounded)
						{
							girl.crawl ();
						}
					}
				}
				
				if(touch.phase != TouchPhase.Ended)
				{
					if(inSpecialWord)
					{
						foreach(SpecialWords sw in holdSpecialWords)
						{	
							Vector2 touchPos = sw.GlobalToLocal (touch.position);
							if(sw.textRect.Contains (touchPos))
							{
								sw.action ();
							}
						}
					}
					
					if(!inSpecialWord)
					{
						if(girl.isGrounded)
						{
							if (girl.isStanding && !girl.isRunning)
							{ 
								girl.run ();
							}
							
							if(!girl.isStanding && !girl.isCrawling)
							{
								girl.crawl ();
							}
							girl.move (touch.position.x + focus.x);
						}
						
						else
						{
							girl.jumpMove(touch.position.x + focus.x);
						}
					}
				}
				
				if(touch.phase == TouchPhase.Ended)
				{
			
					if(!inSpecialWord)
					{
						if(girl.isGrounded)
						{
							girl.idle ();
						}
						else
						{
							girl.jumpMove(girl.x + focus.x);
						}

					}
					if (inSpecialWord)
					{
						inSpecialWord = false;
					}
					
				}
		}
	}
}