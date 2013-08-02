using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;

// remember to extend FMultiTouchableInterface! (wow that's a long title)
public class MainGame: MonoBehaviour, FMultiTouchableInterface 
{
	Vector2 deltaSwipe;
	
	Girl girl;
	float groundHeight;

	FSprite background;
	Ground ground;
	FSprite obstacle1;
	FLabel power1;
	FSprite introText;
	public float scaleGirl;
	
	List<Rect> textRects;
	List<Rect> mediumTextRects;
	List <Rect> pictureObstacleRects;
	List <Rect> specialWordRects;
	
	bool directionTouchFound;
	
	List <SpecialWords> specialWords = new List<SpecialWords> ();
	List <MediumText> mediumText = new List<MediumText>();
	List <FSprite> obstacles=new List<FSprite>();
	List <PictureObstacle> pictures = new List<PictureObstacle>();
	
	FCamObject cam;
	FNode focus;
	
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
		
		textRects = new List<Rect>();
		mediumTextRects = new List<Rect>();
		pictureObstacleRects = new List<Rect>();
		specialWordRects = new List<Rect>();
		
		LoadTextures ();
		SetUpStage ();
		
	}
	
	void LoadTextures()
	{
		// Load the girl 
		GSpineManager.LoadSpine("girlAtlas", "Atlases/girlJson", "Atlases/girlAtlas");
		girl = new Girl("girlAtlas");
		scaleGirl=0.6f;
		girl.scale =scaleGirl;
		girl.alpha = .75f * girl.alpha;
		girl.SetPosition(900, groundHeight);
		
		//load the atlas
		Futile.atlasManager.LoadAtlas("Atlases/AliceAtlas");
		Futile.atlasManager.LoadFont("PalatinoMedium", "MediumNormalText", "Atlases/MediumNormalText", 0, 0);
		Futile.atlasManager.LoadFont("PalatinoSpecial", "MediumSpecialText", "Atlases/MediumSpecialText", 0, 0);
	
		//load sprites
		background = new FSprite("AliceBG1");
		introText = new FSprite("1");
		ground = new Ground(groundHeight);
		obstacle1 = new FSprite("2");
		//power1 = new FLabel("PalitinoMedium", "JUMP!");
	}
	
	void SetUpStage()
	{
		setUpTutorialStage();
		//girl
		
		Futile.stage.AddChild(girl);
		girl.SetPosition(0, groundHeight);
		girl.setGroundHeight (groundHeight);
		//girl.Pause();	
		
		
		
		// makes it so that the entire screen is capable of multitouch
		Futile.touchManager.AddMultiTouchTarget (this);
		
		
		//set camera to follow girl
		cam.setWorldBounds(new Rect(-1.5f * Futile.screen.width, -.5f*Futile.screen.height, 4*Futile.screen.width, 1.25f* Futile.screen.height));
		cam.follow(focus);
		
		
	}
	
	void Update()
	{
		girl.Update();
		bool solid;
		foreach (Rect text in textRects)
		{
			girl.checkCollisions(text);
		}
		
		for(int x = pictures.Count-1; x>=0; x--)
		{
			solid = pictures[x].isSolid ();
			girl.checkCollisions (pictureObstacleRects[x], solid);
		}
		
		for(int x = specialWords.Count-1; x>=0; x--)
		{
			solid = specialWords[x].isSolid ();
			girl.checkCollisions (specialWordRects[x], solid);
		}
		
		for(int x = mediumText.Count-1; x>=0; x--)
		{
			solid = mediumText[x].isSolid ();
			girl.checkCollisions (mediumTextRects[x], solid);
		}
		
		girl.checkCollisions(groundHeight);
		
		focus.x = girl.x - Futile.screen.halfWidth;
		focus.y = girl.y - .1f * Futile.screen.height;
	}
	
	/*-----------------------------------------
	 * Exactly what it says on the tin. Loops
	 * through the touches and does stuff 
	 * with it. Yeah.
	 * ---------------------------------------*/
	public void HandleMultiTouch(FTouch[] touches)
	{

		foreach(FTouch touch in touches)
		{
			if(touch.phase == TouchPhase.Began)
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
				if(girl.isGrounded)
				{
					girl.move (touch.position.x + focus.x);
				}
				
				else
				{
					girl.jumpMove(touch.position.x + focus.x);
				}
			}
			
			if(touch.phase == TouchPhase.Ended)
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
			
		}
	}
	
	void setUpTutorialStage()
	{
		//background
		Futile.stage.AddChild(background);
		background.scale = 0.6f;
		background.SetPosition(background.width/2, background.height/2);
	
		FSprite background2 = new FSprite("blank");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition(background.width + (background2.width/2)-20, background.height/2);
		
		Futile.stage.AddChild (introText);
		
		FSprite obstacle2 = new FSprite("3");
		FSprite obstacle3 = new FSprite("4");
		FSprite obstacle4 = new FSprite("5");
		FSprite obstacle5 = new FSprite("6");
		FSprite obstacle6 = new FSprite("7");
		FSprite obstacle7 = new FSprite("8");
		FSprite obstacle8 = new FSprite("9");
		FSprite obstacle9 = new FSprite("10");
		FSprite obstacle10 = new FSprite("11");
		FSprite obstacle11 = new FSprite("12");
		
		obstacles.Add (obstacle1);
		obstacles.Add (obstacle2);
		obstacles.Add (obstacle3);
		obstacles.Add (obstacle4);
		obstacles.Add (obstacle5);
		obstacles.Add (obstacle6);
		obstacles.Add (obstacle7);
		obstacles.Add (obstacle8);
		obstacles.Add (obstacle9);
		obstacles.Add (obstacle10);
		obstacles.Add (obstacle11);
		
		//ground text
		
		ground.Start();
		girl.Start();
		
		//obstacle
		foreach (FSprite i in obstacles)
		{
			Futile.stage.AddChild (i);
			i.scale = 0.65f;
		}
		
		obstacle1.SetPosition(obstacle1.width*4.5f, groundHeight + (obstacle1.height/2f));
		obstacle3.SetPosition (obstacle1.x + obstacle3.width/2, obstacle1.y + obstacle3.height);
		obstacle2.SetPosition (obstacle3.x, obstacle3.y+obstacle2.height);
		obstacle4.SetPosition (obstacle3.x+obstacle4.width, obstacle3.y);
		
		obstacle5.SetPosition(obstacle4.x, obstacle2.y + obstacle5.height);
		obstacle6.SetPosition(obstacle5.x, obstacle5.y+obstacle6.height);
		obstacle7.SetPosition (obstacle6.x+obstacle7.width, obstacle6.y);
		obstacle8.SetPosition (obstacle7.x+obstacle8.width*2f, obstacle7.y);
		obstacle9.SetPosition (obstacle8.x+obstacle9.width, obstacle8.y);
		obstacle10.SetPosition (obstacle9.x, obstacle5.y);
		obstacle11.SetPosition (obstacle10.x, obstacle2.y);
		
		foreach (FSprite obs in obstacles)
		{
			Rect obsRect = obs.localRect.CloneAndOffset(obs.x, obs.y);
			textRects.Add(obsRect);
		}
		
		PictureObstacle hole = new PictureObstacle("Hole");
		Futile.stage.AddChild (hole);
		hole.scale=0.5f;
		hole.SetPosition (obstacle7.x+(obstacle8.x-obstacle7.x)/2f, obstacle7.y+hole.height/4.5f);
		hole.setSolidity (false);
		pictures.Add (hole);
		
		foreach (PictureObstacle pic in pictures)
		{
			Rect picRect = pic.localRect.CloneAndOffset(pic.x, pic.y);
			pictureObstacleRects.Add(picRect);
		}
		
		introText.SetPosition (introText.width/1.7f, Futile.screen.height*0.8f);
		Rect introTextRect = introText.localRect.CloneAndOffset(introText.x, introText.y);
		textRects.Add(introTextRect);
		
		
		MediumText sentence1 = new MediumText("PalatinoMedium", "In another moment");
		MediumText sentence2 = new MediumText("PalatinoMedium", "went Alice after it");
		MediumText sentence3 = new MediumText("PalatinoMedium", "never once considering");
		MediumText sentence4 = new MediumText("PalatinoMedium", "how in the world");
		MediumText sentence5 = new MediumText("PalatinoMedium", "she was going");
		MediumText sentence6 = new MediumText("PalatinoMedium", "to get");
		
		mediumText.Add (sentence1);
		mediumText.Add (sentence2);
		mediumText.Add (sentence3);
		mediumText.Add (sentence4);
		mediumText.Add (sentence5);
		mediumText.Add (sentence6);
		
		foreach (MediumText i in mediumText)
		{
			Futile.stage.AddChild (i);
			i.scale = 0.6f;
		}
		
		sentence1.SetPosition (obstacle3.x+sentence1.textRect.width/1.9f, obstacle4.y+ obstacle4.height/2f+sentence1.textRect.height*0.1f);
		
		ChangeWord down = new ChangeWord("PalatinoSpecial", " down");
		Futile.stage.AddChild(down);
		down.SetPosition (sentence1.x + down.textRect.width*1.2f, sentence1.y);
		
		sentence2.SetPosition(down.x+sentence2.textRect.width/2.1f, down.y);
		sentence3.SetPosition (sentence2.x+sentence3.textRect.width/2.3f, sentence2.y-sentence3.textRect.height/2f);
		sentence4.SetPosition(sentence3.x, sentence3.y-sentence4.textRect.height/2f);
		sentence5.SetPosition (sentence4.x, sentence4.y-sentence5.textRect.height/2f);
		sentence6.SetPosition (sentence5.x, sentence5.y-sentence6.textRect.height/2f);
		
		float outX = sentence6.x + obstacle1.width;
		float outY = girl.y;
		Vector2 outPosition = new Vector2(outX, outY);
		ChangeGirlPositionWord Out = new ChangeGirlPositionWord("PalatinoSpecial", "out", outPosition, girl);
		Futile.stage.AddChild (Out);
		Out.SetPosition(sentence6.x, sentence6.y-Out.textRect.height);
		
		foreach (MediumText mt in mediumText)
		{
			Rect mtRect = new Rect(mt.x, mt.y, 
			mt.textRect.height, mt.textRect.width);
			mediumTextRects.Add (mtRect);
		}
		
		specialWords.Add (Out);
		specialWords.Add (down);
		
		foreach (SpecialWords sw in specialWords)
		{
			Rect swRect = new Rect(sw.x, sw.y, 
			sw.textRect.height, sw.textRect.width);
			specialWordRects.Add (swRect);
		}
		
	}
	
}