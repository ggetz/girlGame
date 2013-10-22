using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;

// remember to extend FMultiTouchableInterface! (wow that's a long title)
public class MainGame: MonoBehaviour, FMultiTouchableInterface 
{
	Vector2 deltaSwipe;
	
	public AudioClip music;
	
	Girl girl;
	
	Eraser eraser;
	
	float groundHeight;

	FSprite background;
	Ground ground;
	public float scaleGirl;
	
	List <MediumText> twinkleText = new List<MediumText>();
	List <MovingPictureObstacles> updateObs = new List<MovingPictureObstacles>();
	List<Rectangle> collisionRects = new List<Rectangle>();
	List<Rectangle> obstacleCollisions=new List<Rectangle>();
	
	bool directionTouchFound;
	bool inSpecialWord=false;
	
	List <SpecialWords> specialWords = new List<SpecialWords> ();
	List <MediumText> mediumText = new List<MediumText>();
	List <SpecialWords> holdSpecialWords = new List<SpecialWords> ();
	List <MovingPictureObstacles> movingObs=new List<MovingPictureObstacles>();
	
	FCamObject cam;
	FNode focus;
	
	string blockFont;
	string specialFont;
	
	float mediumTextScale=0.6f;
	Doodle temp;
	
	List<FSprite> layer2;
	List<FSprite> layer3;
	float layer2speed = .1f;
	float layer3speed = .2f;
	
	int doodleCollect=60;
	bool collectionTime=false;
	
	void Start()
	{
		// Setup Futile
		FutileParams fparams = new FutileParams(true, true, false, false);
		fparams.AddResolutionLevel(1024.0f,	1.0f, 1.0f, "");
		fparams.origin = new Vector2(0f, 0f);
		Futile.instance.Init(fparams);
		
		groundHeight = 0.25f * Futile.screen.halfHeight;
		
		Debug.Log ("GroundHeight: " + groundHeight);
		//set up camera
		Futile.stage.AddChild(cam = new FCamObject());
		focus = new FNode();
		
		layer2 = new List<FSprite>();
		layer3 = new List<FSprite>();
		
		LoadTextures ();
		SetUpStage ();
		
		foreach (MediumText t in mediumText)
		{
			collisionRects.Add(new Rectangle(t, mediumTextScale));	
		}
		foreach (MovingPictureObstacles pic in movingObs)
		{
			collisionRects.Add (pic.getRect());
		}
		
	}
	
	void LoadTextures()
	{
		// Load the girl 
		GSpineManager.LoadSpine("girlAtlas", "Atlases/girlJson", "Atlases/girlAtlas");
		scaleGirl=0.6f;
		girl = new Girl("girlAtlas", scaleGirl);
		girl.scale =scaleGirl;
		girl.alpha = .75f * girl.alpha;
		girl.SetPosition(1200, groundHeight);
		
		//load the atlas
		Futile.atlasManager.LoadAtlas("Atlases/AliceAtlas");
		Futile.atlasManager.LoadFont("PalatinoMedium", "MediumNormalText", "Atlases/MediumNormalText", 0, 0);
		Futile.atlasManager.LoadFont("PalatinoSpecial", "MediumSpecialText", "Atlases/MediumSpecialText", 0, 0);
		blockFont = "PalatinoMedium";
		specialFont = "PalatinoSpecial";
		
		//load sprites
		ground = new Ground(groundHeight, "PalatinoMedium", "Assets/Resources/Atlases/AliceGround.txt", 2);
		//power1 = new FLabel("PalitinoMedium", "JUMP!");
		
		GSpineManager.LoadSpine("EraserAtlas", "Atlases/EraserJson", "Atlases/EraserAtlas");
		eraser = new Eraser("EraserAtlas", girl, 0.6f, 5);
		eraser.SetPosition(girl.x, Futile.screen.height + girl.y);
		eraser.scale=0.6f;
		
	}
			
	void SetUpStage()
	{
		setUpTutorialStage();
		//girl
		
		Futile.stage.AddChild(girl);
		Futile.stage.AddChild (eraser);
		girl.SetPosition(0, groundHeight);
		girl.setGroundHeight (groundHeight);
		
		//ground text
		ground.Start();
		
		girl.Start();
		// makes it so that the entire screen is capable of multitouch
		Futile.touchManager.AddMultiTouchTarget (this);
		
		//set camera to follow girl
		cam.setWorldBounds(new Rect(-1.5f * Futile.screen.width, -.5f*Futile.screen.height, 30*Futile.screen.width, 1.25f* Futile.screen.height));
		cam.follow(focus);
	}
	
	void Update()
	{
		
		girl.Update(collisionRects); 
		foreach (MediumText txt in twinkleText)
		{
			txt.Update();
		}
		
		foreach (MovingPictureObstacles pic in movingObs)
		{
			pic.Update ();
			collisionRects.RemoveAt(collisionRects.Count-1);
		}
		
		foreach(MovingPictureObstacles pic in movingObs)
		{
			collisionRects.Add(pic.getRect ());
		}
		
		focus.x = girl.x - Futile.screen.halfWidth;
		focus.y = girl.y - .1f * Futile.screen.height;
		
		//eraser.Update ();
		
		if(girl.getLife ()==0)
		{
			Application.LoadLevel ("DeadScreen");
		}
		
		if(girl.checkDoodleCollision ())
		{
			if(temp==null)
			{
				temp=girl.getCollidedDoodle ();
			}
			
			collectionTime=true;
		}
		
		if(collectionTime)
		{
			doodleCollect--;
			
			if(doodleCollect==0)
			{
				Debug.Log ("Goodbye friends I am gone");
				Futile.stage.RemoveChild (girl.getCollidedDoodle ());
				temp=null;
				doodleCollect=60;
				collectionTime=false;
			}
		}
		updateBackground();
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
				for(int i = specialWords.Count-1; i>=0; i--)
					{
						SpecialWords word = specialWords[i];
						Vector2 touchPos = word.GlobalToLocal (touch.position);
						if(word.textRect.Contains (touchPos))
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
		
	void setUpTutorialStage()
	{
		//background
		FSprite b = makeBackground(-546);
		
		background = makeBackground();
		FSprite background2 = makeBackground(background.width + (background.width/2)-20);
		
		//obstacles
	// and fortunately was just in time to see it pop down a large rabbit-hole under the hedge.
		MediumText blocka1 = new MediumText(blockFont, "Alice started to her");
		MediumText blocka2 = new MediumText(blockFont, "feet, for it flashed");
		MediumText blocka3 = new MediumText(blockFont, "across her mind that");
		MediumText blocka4 = new MediumText(blockFont, "she had never before");
		MediumText blocka5 = new MediumText(blockFont, "seen any rabbit with");
		MediumText blocka6 = new MediumText(blockFont, "either a waist -coat");
		MediumText blocka7 = new MediumText(blockFont, "pocket,or a watch to");
		MediumText blocka8 = new MediumText(blockFont, "take out of it , and");
		MediumText blocka9 = new MediumText(blockFont, "burning with curiousity,she ran across");
		MediumText blocka10 = new MediumText(blockFont, "the field after it, and , fortunately was"); 
		MediumText blocka11 = new MediumText(blockFont, "just in time to see it pop  down a large ");

		mediumText.Add (blocka1);
		mediumText.Add (blocka2);
		mediumText.Add (blocka3);
		mediumText.Add (blocka4);
		mediumText.Add (blocka5);
		mediumText.Add (blocka6);
		mediumText.Add (blocka7);
		mediumText.Add (blocka8);
		mediumText.Add (blocka9);
		mediumText.Add (blocka10);
		mediumText.Add (blocka11);
		
		GSpineManager.LoadSpine("DoodleAtlas", "Atlases/DoodleJson", "Atlases/DoodleAtlas");
		Doodle rabbit = new Doodle("DoodleAtlas", 193f, 200f, 0.5f);
		rabbit.SetSkin ("Bunny");

		MediumText blockb = new MediumText(blockFont, "rabbit-hole under the hedge");
		mediumText.Add (blockb);
		
		MediumText blockc1 = new MediumText(blockFont, "There was nothing so VERY remarkable");
		MediumText blockc2 = new MediumText(blockFont, "in that; nor did Alice think it so ");
		MediumText blockc3 = new MediumText(blockFont, "VERY much out of the  ");
		MediumText blockc4 = new MediumText(blockFont, "way to hear the Rabbit");
		MediumText blockc5 = new MediumText(blockFont, "say 'Oh dear! Oh dear!");
		MediumText blockc6 = new MediumText(blockFont, " I shall be late!'    ");
		
		mediumText.Add (blockc1);
		mediumText.Add (blockc2);
		mediumText.Add (blockc3);
		mediumText.Add (blockc4);
		mediumText.Add (blockc5);
		mediumText.Add (blockc6);
		
		PictureObstacle hole = new PictureObstacle("Hole");
		Futile.stage.AddChild (hole);
		hole.scale=0.5f;
		
		hole.setSolidity (false);
		
		MediumText sentence1 = new MediumText(blockFont, "In another moment");
		MediumText sentence2 = new MediumText(blockFont, " went Alice after it never once considering");
		MediumText sentence3 = new MediumText(blockFont, "how in the world she");
		MediumText sentence4 = new MediumText(blockFont, "was going to get");
		
		mediumText.Add (sentence1);
		mediumText.Add (sentence2);
		mediumText.Add (sentence3);
		mediumText.Add (sentence4);

		blocka1.SetPosition (Futile.screen.width*0.5f, blocka1.textRect.height*7.5f + groundHeight);
		blocka2.SetPosition (blocka1.x + blocka2.textRect.width*3f, blocka1.y);
		blocka3.SetPosition (blocka1.x, blocka1.y - blocka3.textRect.height/1.5f);
		blocka4.SetPosition (blocka2.x, blocka2.y - blocka4.textRect.height/1.5f);
		blocka5.SetPosition (blocka1.x, blocka3.y - blocka5.textRect.height/1.5f);
		blocka6.SetPosition (blocka2.x, blocka4.y - blocka6.textRect.height/1.5f);
		blocka7.SetPosition (blocka1.x, blocka5.y - blocka7.textRect.height/1.5f);
		blocka8.SetPosition (blocka2.x, blocka6.y - blocka8.textRect.height/1.5f);
		blocka9.SetPosition (blocka1.x, blocka8.y - blocka9.textRect.height);
		blocka10.SetPosition (blocka9.x, blocka9.y - blocka10.textRect.height);
		blocka11.SetPosition (blocka9.x, blocka10.y - blocka11.textRect.height);
		
		blockb.SetPosition (blocka11.x - blockb.textRect.width/2f, groundHeight+(blocka11.y-groundHeight)/2f);
		rabbit.SetPosition (blockb.x + blockb.textRect.width/2f, groundHeight);
		
		blockc5.SetPosition(blocka1.x + blockc5.textRect.width/2f, blocka1.y + blockc5.textRect.height/2f);
		blockc6.SetPosition(blockc5.x + blockc6.textRect.width*2.5f, blockc5.y);
		blockc3.SetPosition (blockc5.x, blockc5.y + blockc3.textRect.height);
		blockc4.SetPosition (blockc6.x, blockc6.y+blockc4.textRect.height);
		blockc1.SetPosition (blockc5.x+(blockc1.textRect.width/2f - blockc3.textRect.width/2f), blockc1.textRect.height + blockc3.y);
		blockc2.SetPosition (blockc6.x - + (blockc2.textRect.width/2f - blockc4.textRect.width/2f), blockc2.textRect.height + blockc4.y);
		
		sentence1.SetPosition (blocka7.x + sentence1.textRect.width/1.5f, blocka9.y+sentence1.textRect.height/1.5f);
		
		ChangeWord down = new ChangeWord(specialFont, "down", 1f);
		Futile.stage.AddChild(down);
		down.SetPosition (sentence1.x + down.textRect.width*1.5f, sentence1.y);
		
		Futile.stage.AddChild (rabbit);
		rabbit.Start();
		girl.addDoodle(rabbit);
		
		sentence2.SetPosition(down.x+sentence2.textRect.width/2.5f, down.y);
		sentence3.SetPosition (sentence2.x+(sentence2.textRect.width-sentence3.textRect.width)/4f, sentence2.y-sentence3.textRect.height);
		sentence4.SetPosition(sentence3.x, sentence3.y-sentence4.textRect.height);
		
		
		hole.SetPosition ((blockc1.x+blockc2.x)/2f, blockc1.y);
		
		float outX = sentence4.x+girl.girlWidth*4f;
		float outY = girl.y;
		Vector2 outPosition = new Vector2(outX, outY);
		ChangeGirlPositionWord Out = new ChangeGirlPositionWord(specialFont, "out", 1f, outPosition, girl);
		Futile.stage.AddChild (Out);
		Out.SetPosition(sentence4.x, groundHeight + (sentence4.y-groundHeight)/2f);
		
		specialWords.Add (Out);
		specialWords.Add (down);
		
		setUpFiller1(background2.x + background2.width/2f);
		
	}
	
	void setUpFiller1(float startX)
	{
		//I'm sure I shan't be able! I shall be a great deal too far off
		FSprite background1 = makeBackground(startX + background.width/2);
		FSprite background2 = makeBackground(startX + background1.width + (background.width/2)-20);
		
		MediumText blockd1 = new MediumText(blockFont, "'Curiouser and curiouser!'");
		MediumText blockd2 = new MediumText(blockFont, "cried Alice (she was");
		MediumText blockd3 = new MediumText(blockFont, "so much surprised, that");
		
		MediumText blockd4 = new MediumText(blockFont, "for the moment she");
		MediumText blockd5 = new MediumText(blockFont, "quite forgot how to speak good English);");
		
		MediumText blockd6 = new MediumText(blockFont, "'now I'm opening out like the largest telescope that ever was! Good-bye, feet!'");
		MediumText blockd7 = new MediumText(blockFont, "(for when she looked down at her feet, they seemed to be almost out of sight,");
		MediumText blockd8 = new MediumText(blockFont, " they were getting so far off). 'Oh, my poor little feet, I wonder who will put");
		
		MediumText blockd9 = new MediumText(blockFont, "on your shoes and stockings");
		MediumText blockd10 = new MediumText(blockFont, "for you now, dears? I'm sure"); 
		MediumText blockd11 = new MediumText(blockFont, "I shan't be able! I shall be");
		MediumText blockd12 = new MediumText(blockFont, "a great deal too far off");
		blockd1.SetPosition (startX + blockd1.textRect.width/2f, Futile.screen.height*0.3f);
		MediumText blockd13 = new MediumText(blockFont, "to trouble myself");
		MediumText blockd14 = new MediumText(blockFont, "about you: you must");
		
		MediumText blockd15 = new MediumText(blockFont, "manage the best way you can; - but I");
		MediumText blockd16 = new MediumText(blockFont, "must be kind to them,' thought Alice,");
		
		blockd1.SetPosition (startX + Futile.screen.width*0.01f, Futile.screen.height*0.3f);
		blockd2.SetPosition (blockd1.x, blockd1.y - blockd1.textRect.height*0.6f);
		blockd3.SetPosition (blockd1.x, blockd2.y - blockd2.textRect.height*0.6f);
		blockd4.SetPosition (blockd1.x + blockd5.textRect.width*0.6f, blockd1.y + blockd4.textRect.height*3f);
		blockd5.SetPosition (blockd4.x, blockd4.y - blockd5.textRect.height*0.6f);
		blockd6.SetPosition (blockd4.x + blockd6.textRect.width*0.5f, blockd5.y + blockd6.textRect.height*4f);
		blockd7.SetPosition (blockd6.x, blockd6.y - blockd7.textRect.height*0.6f);
		blockd8.SetPosition (blockd7.x, blockd7.y - blockd8.textRect.height*0.6f);
		blockd9.SetPosition (blockd5.x + blockd9.textRect.width, blockd8.y - blockd9.textRect.height*5f);
		blockd10.SetPosition (blockd9.x, blockd9.y - blockd10.textRect.height*0.6f);
		blockd11.SetPosition (blockd10.x, blockd10.y - blockd11.textRect.height*0.6f);
		blockd12.SetPosition (blockd6.x + blockd12.textRect.width*0.5f, blockd6.y + blockd12.textRect.height*3f);
		blockd13.SetPosition (blockd6.x + blockd6.textRect.width*0.5f, blockd6.y - blockd13.textRect.height*3f);
		blockd14.SetPosition (blockd13.x, blockd13.y - blockd14.textRect.height*0.6f);
		blockd15.SetPosition (blockd6.x + blockd6.textRect.width*0.4f, blockd14.y - blockd15.textRect.height*2f);
		blockd16.SetPosition (blockd15.x, blockd15.y - blockd16.textRect.height*0.6f);
		
		mediumText.Add (blockd1);
		mediumText.Add (blockd2);
		mediumText.Add (blockd3);
		mediumText.Add (blockd4);
		mediumText.Add (blockd5);
		mediumText.Add (blockd6);
		mediumText.Add (blockd7);
		mediumText.Add (blockd8);
		mediumText.Add (blockd9);
		mediumText.Add (blockd10);
		mediumText.Add (blockd11);
		mediumText.Add (blockd12);
		mediumText.Add (blockd13);
		mediumText.Add (blockd14);
		mediumText.Add (blockd15);
		mediumText.Add (blockd16);
		
		setUpShrinkStage (background2.x+background2.width/2f);
	
	}
	
	void setUpShrinkStage(float startX)
	{
		FSprite background3 = makeBackground(startX + background.width/2 - 20);
		FSprite background4 = makeBackground(background3.x +(background3.width/2)-20);
				
		PictureObstacle drinkMe = new PictureObstacle("drinkme");
		drinkMe.scale=0.6f;
	
		ChangeGirlSizeWord shrink = new ChangeGirlSizeWord(specialFont, "shrink", 1f, 0.3f, girl);
		ChangeGirlSizeWord grow = new ChangeGirlSizeWord(specialFont, "grow", 1f, 0.6f, girl);
		
		specialWords.Add (shrink);
		specialWords.Add (grow);
		Futile.stage.AddChild (grow);
		Futile.stage.AddChild (shrink);
		
		MediumText sentence1 = new MediumText(blockFont, "First, however, she waited for a few minutes to see if");
		MediumText sentence2 = new MediumText(blockFont, "she was going to ");
		MediumText sentence3 = new MediumText(blockFont, " any further");
		
		MediumText blocka1 = new MediumText(blockFont, "she  felt a little");
		MediumText blocka2 = new MediumText(blockFont, "nervous about this");
		MediumText blocka3 = new MediumText(blockFont, "' for it might end"); 
		MediumText blocka4 = new MediumText( blockFont, "you know,' said Alice to herself, ' in my going out altogether, like a"); 
		MediumText blocka5 = new MediumText(blockFont, "candle. I wonder what I should be like then?' And she tried to fancy");
		MediumText blocka6 = new MediumText(blockFont, "what the flame of a candle is like after the candle is blown out, for");
		MediumText blocka7 = new MediumText (blockFont, "she could not remember ever having seen such a thing. After a while,");
		MediumText blocka8 = new MediumText (blockFont, "finding that nothing happened she decided on going into the garden at");
		MediumText blocka9 = new MediumText(blockFont, "once; but, alas for poor Alice! when she got to the door she found she");
		
		mediumText.Add (sentence1);
		mediumText.Add (sentence2);
		mediumText.Add (sentence3);
	
		mediumText.Add (blocka1);
		mediumText.Add (blocka2);
		mediumText.Add (blocka3);
		mediumText.Add (blocka4);
		mediumText.Add (blocka5);
		mediumText.Add (blocka6);
		mediumText.Add (blocka7);
		mediumText.Add (blocka8);
		mediumText.Add (blocka9);
		
		sentence1.SetPosition (startX + sentence1.textRect.width/3f, Futile.screen.height*0.85f);
		sentence3.SetPosition(sentence1.x + (sentence1.textRect.width - sentence3.textRect.width)/4f, sentence1.y - sentence3.textRect.height);
		shrink.SetPosition(sentence3.x - shrink.textRect.width, sentence3.y);
		sentence2.SetPosition (shrink.x - sentence2.textRect.width/2f, sentence3.y);
		
		blocka1.SetPosition (sentence3.x, sentence3.y - blocka1.textRect.height);
		blocka2.SetPosition (sentence3.x, blocka1.y - blocka2.textRect.height);
		blocka3.SetPosition (sentence3.x, blocka2.y - blocka3.textRect.height);
		blocka4.SetPosition (sentence1.x, blocka3.y - blocka4.textRect.height);
		blocka5.SetPosition (sentence1.x, blocka4.y - blocka5.textRect.height);
		blocka6.SetPosition (sentence1.x, blocka5.y - blocka6.textRect.height);
		blocka7.SetPosition (sentence1.x, blocka6.y - blocka7.textRect.height);
		blocka8.SetPosition (sentence1.x, blocka7.y - blocka8.textRect.height);
		blocka9.SetPosition (sentence1.x, blocka8.y - blocka9.textRect.height);
		
		MediumText sentence4 = new MediumText(blockFont, "\"Well, I'll eat it,\" said Alice");
		MediumText sentence5 = new MediumText(blockFont, "\"and if it makes me ");
		MediumText sentence6 = new MediumText(blockFont, " larger, I can reach the key.\"");
		
		mediumText.Add (sentence4);
		mediumText.Add (sentence5);
		mediumText.Add (sentence6);
		
		sentence4.SetPosition (blocka1.x + sentence4.textRect.width, blocka2.y);
		sentence5.SetPosition (sentence4.x, sentence4.y - sentence5.textRect.height);
		grow.SetPosition (sentence5.x - (sentence5.textRect.width - grow.textRect.width)/4f, sentence5.y - grow.textRect.height);
		sentence6.SetAnchor (grow.x + sentence6.textRect.width/2f, grow.y);
		
		drinkMe.SetPosition(grow.x, groundHeight + drinkMe.height/2f);
		Futile.stage.AddChild (drinkMe);
		setUpFiller2(background4.x + background4.width/2f);
		
	}

	void setUpFiller2(float startX)
	{
		//'Oh dear, what nonsense I'm ... along in a great hurry, muttering
		FSprite background1 = makeBackground(startX + background.width/2);
		FSprite background2 = makeBackground(startX + background1.width + (background.width/2)-20);
		FSprite background3 = makeBackground(startX + background1.width + background.width + (background.width/2)-20);

		
		GSpineManager.LoadSpine("DoodleAtlas", "Atlases/DoodleJson", "Atlases/DoodleAtlas");
		Doodle key = new Doodle("DoodleAtlas", 150f, 179f, 0.6f);
		key.SetSkin ("Key");
		
		MediumText blockd1 = new MediumText(blockFont, "Oh dear, what");
		MediumText blockd2 = new MediumText(blockFont, "nonsense I'm");
		
		MediumText blockd3 = new MediumText(blockFont, "talking!' Just then her head struck against");
		MediumText blockd4 = new MediumText(blockFont, "the roof of the hall: in fact she was now more");
		MediumText blockd5 = new MediumText(blockFont, "than nine feet high, and she at once took up");
		
		MediumText blockd6 = new MediumText(blockFont, "the little golden key and");
		MediumText blockd7 = new MediumText(blockFont, "hurried off to the garden");
		
		MediumText blockd8 = new MediumText(blockFont, "door. Poor Alice!");
		MediumText blockd9 = new MediumText(blockFont, "It was as much");
		MediumText blockd10 = new MediumText(blockFont, "as she could do,"); 
		
		MediumText blockd11 = new MediumText(blockFont, "lying down on one side, to look through into the garden with one eye; but");
		MediumText blockd12 = new MediumText(blockFont, "to get through was more hopeless than ever: she sat down and began to cry");
		
		MediumText blockd13 = new MediumText(blockFont, "again. 'You ought to be ashamed of yourself,'");
		MediumText blockd14 = new MediumText(blockFont, "said Alice, 'a great girl like you,' (she might");
		
		MediumText blockd15 = new MediumText(blockFont, "well say this), 'to go on crying in this");
		MediumText blockd16 = new MediumText(blockFont, "way! Stop this moment, I tell you!' But");
		MediumText blockd17 = new MediumText(blockFont, "she went on all the same, shedding gallons");
		MediumText blockd18 = new MediumText(blockFont, "of tears, until there was a large pool all");
		MediumText blockd19 = new MediumText(blockFont, "round her, about four inches deep and reaching");
		MediumText blockd20 = new MediumText(blockFont, "half down the hall. After a time she heard a");
		
		MediumText blockd21 = new MediumText(blockFont, "little pattering of");
		MediumText blockd22 = new MediumText(blockFont, "feet in the distance");
		
		MediumText blockd23 = new MediumText(blockFont, "and she hastily dried her eyes to see what was coming.");
		MediumText blockd24 = new MediumText(blockFont, "It was the White Rabbit returning, splendidly dressed,");
		MediumText blockd25 = new MediumText(blockFont, "with a pair of white kid gloves in one hand and a large");
		
		MediumText blockd26 = new MediumText(blockFont, "fan in the other: he came trotting");
		MediumText blockd27 = new MediumText(blockFont, "along in a great hurry, muttering");
		
		blockd1.SetPosition (startX, Futile.screen.height*0.9f);
		blockd2.SetPosition (blockd1.x, blockd1.y - blockd1.textRect.height*0.6f);
		blockd3.SetPosition (blockd1.x, blockd1.y - blockd3.textRect.height*3.5f);
		blockd4.SetPosition (blockd3.x, blockd3.y - blockd4.textRect.height*0.6f);
		blockd5.SetPosition (blockd4.x, blockd4.y - blockd5.textRect.height*0.6f);
		blockd6.SetPosition (blockd3.x + blockd3.textRect.width*0.5f, blockd3.y - blockd3.textRect.height*4f);
		blockd7.SetPosition (blockd6.x, blockd6.y - blockd7.textRect.height*0.6f);
		blockd8.SetPosition (blockd6.x + blockd6.textRect.width*0.6f, blockd6.y + blockd8.textRect.height*2f);
		blockd9.SetPosition (blockd8.x, blockd8.y - blockd9.textRect.height*0.6f);
		blockd10.SetPosition (blockd9.x, blockd9.y - blockd10.textRect.height*0.6f);
		blockd11.SetPosition (blockd7.x + blockd11.textRect.width*0.5f, blockd7.y - blockd11.textRect.height*1.5f);
		blockd12.SetPosition (blockd11.x, blockd11.y - blockd12.textRect.height*0.6f);
		blockd13.SetPosition (blockd11.x, blockd8.y + blockd13.textRect.height*4f);
		blockd14.SetPosition (blockd13.x, blockd13.y - blockd14.textRect.height*0.6f);
		blockd15.SetPosition (blockd13.x + blockd15.textRect.width*0.9f, blockd13.y - blockd15.textRect.height*2f);
		blockd16.SetPosition (blockd15.x, blockd15.y - blockd16.textRect.height*0.6f);
		blockd17.SetPosition (blockd16.x, blockd16.y - blockd17.textRect.height*0.6f);
		blockd18.SetPosition (blockd17.x, blockd17.y - blockd18.textRect.height*0.6f);
		blockd19.SetPosition (blockd18.x, blockd18.y - blockd19.textRect.height*0.6f);
		blockd20.SetPosition (blockd19.x, blockd19.y - blockd20.textRect.height*0.6f);
		blockd21.SetPosition (blockd15.x + blockd21.textRect.width*1.5f, blockd15.y + blockd21.textRect.height*4f);
		blockd22.SetPosition (blockd21.x, blockd21.y - blockd22.textRect.height*0.6f);
		blockd23.SetPosition (blockd15.x + blockd23.textRect.width*0.6f, blockd15.y - blockd23.textRect.height*4.5f);
		blockd24.SetPosition (blockd23.x, blockd23.y - blockd24.textRect.height*0.6f);
		blockd25.SetPosition (blockd24.x, blockd24.y - blockd25.textRect.height*0.6f);
		blockd26.SetPosition (blockd23.x + blockd26.textRect.width*0.8f, blockd23.y + blockd26.textRect.height*3f);
		blockd27.SetPosition (blockd26.x, blockd26.y - blockd27.textRect.height*0.6f);
		
		key.SetPosition (blockd6.x, blockd6.y+key.height/2f);
		Futile.stage.AddChild (key);
		key.Start ();
		girl.addDoodle (key);
		
		mediumText.Add (blockd1);
		mediumText.Add (blockd2);
		mediumText.Add (blockd3);
		mediumText.Add (blockd4);
		mediumText.Add (blockd5);
		mediumText.Add (blockd6);
		mediumText.Add (blockd7);
		mediumText.Add (blockd8);
		mediumText.Add (blockd9);
		mediumText.Add (blockd10);
		mediumText.Add (blockd11);
		mediumText.Add (blockd12);
		mediumText.Add (blockd13);
		mediumText.Add (blockd14);
		mediumText.Add (blockd15);
		mediumText.Add (blockd16);
		mediumText.Add (blockd17);
		mediumText.Add (blockd18);
		mediumText.Add (blockd19);
		mediumText.Add (blockd20);
		mediumText.Add (blockd21);
		mediumText.Add (blockd22);
		mediumText.Add (blockd23);
		mediumText.Add (blockd24);
		mediumText.Add (blockd25);
		mediumText.Add (blockd26);
		mediumText.Add (blockd27);
		
		setUpMushroomStage (background3.x+background3.width/2f);

	}
	
	void setUpMushroomStage(float startX)
	{
		FSprite background1 = makeBackground((background.width/2)-20 + startX);
		FSprite background2 = makeBackground((background.width/2)-20 + background1.x);
		
		FSprite background3 = new FSprite("layer1");
		Futile.stage.AddChild (background3);
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("layer1");
		Futile.stage.AddChild (background4);
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
		
		GSpineManager.LoadSpine("MushroomAtlas", "Atlases/MushroomJson", "Atlases/MushroomAtlas");
		GrowingMushroom smallMushroom = new GrowingMushroom("MushroomAtlas", "small", 0.6f);
		smallMushroom.scale = 0.6f;
		
		GrowingMushroom bigMushroom = new GrowingMushroom("MushroomAtlas", "big", 0.6f);
		bigMushroom.scale = 0.6f;

		GSpineManager.LoadSpine("DoodleAtlas", "Atlases/DoodleJson", "Atlases/DoodleAtlas");
		Doodle mushroomDoodle = new Doodle("DoodleAtlas", 250f, 150f, 0.6f);
		mushroomDoodle.SetSkin ("Mushroom");
		
		AffectPictureWords bgrow = new AffectPictureWords(specialFont, "grow", 1f, smallMushroom);
		bgrow.SetPosition (300, groundHeight + 20f);
		specialWords.Add (bgrow);
		
		AffectPictureWords sgrow = new AffectPictureWords(specialFont, "grow", 1f, bigMushroom);
		specialWords.Add(sgrow);
		
		MediumText sentence1 = new MediumText(blockFont, "'One side will make you");
		MediumText sentence2 = new MediumText(blockFont, " taller, and the other side will make you ");
		MediumText sentence3 = new MediumText(blockFont, " shorter.'");
		
		MediumText block1 = new MediumText(blockFont, "'One side of WHAT? The other side of WHAT?' thought Alice to herself.");
		MediumText block2 = new MediumText(blockFont, "'Of the mushroom,'said");
		MediumText block3 = new MediumText(blockFont, "the Caterpillar, just ");
		MediumText block4 = new MediumText(blockFont, "as if she had asked it");
		MediumText block5 = new MediumText(blockFont, "aloud ; and in another");
		MediumText block6 = new MediumText(blockFont, " moment it was out  of");
		MediumText block7 = new MediumText(blockFont, "sight. Alice remained ");
		MediumText block8 = new MediumText(blockFont, "looking thoughtfully at the mushroom ");
		
		sentence1.SetPosition(startX + sentence1.textRect.width*0.5f, Futile.screen.height*0.4f);
		bgrow.SetPosition (sentence1.x + sentence1.textRect.width/2.3f, sentence1.y);
		sentence2.SetPosition (sentence1.x + (sentence2.textRect.width-sentence1.textRect.width)/4f, sentence1.y - sentence2.textRect.height);
		sgrow.SetPosition (sentence2.x + sentence2.textRect.width/2.6f, sentence2.y);
		sentence3.SetPosition (sgrow.x+sentence3.textRect.width/1.7f, sentence2.y);
		
		bigMushroom.SetPosition (sentence3.x + sentence3.textRect.width*1.5f, groundHeight);
		smallMushroom.SetPosition (bigMushroom.x + smallMushroom.width*1.5f, groundHeight);
		
		bigMushroom.Start ();
		smallMushroom.Start();
		
		block1.SetPosition (smallMushroom.x + block1.textRect.width/2f, bigMushroom.height);
		block2.SetPosition (block1.x - (block1.textRect.width-block2.textRect.width)/4f, block1.y-block2.textRect.height);
		block3.SetPosition (block2.x, block2.y-block3.textRect.height);
		block4.SetPosition (block2.x, block3.y-block4.textRect.height);
		block5.SetPosition (block2.x, block4.y-block5.textRect.height);
		block6.SetPosition (block2.x, block5.y-block6.textRect.height);
		block7.SetPosition (block2.x, block6.y-block7.textRect.height);
		block8.SetPosition (block1.x + block8.textRect.width*0.25f, block1.y + block8.textRect.height*3.5f);
		
		mushroomDoodle.SetPosition (block8.x, block8.y+block8.textRect.height*0.3f);
		Futile.stage.AddChild (mushroomDoodle);
		mushroomDoodle.Start ();
		Debug.Log ("Mushroom: " + mushroomDoodle.doodleRect.left ());
		girl.addDoodle (mushroomDoodle);
		
		mediumText.Add (sentence1);
		mediumText.Add (sentence2);
		mediumText.Add (sentence3);
		
		mediumText.Add (block1);
		mediumText.Add (block2);
		mediumText.Add (block3);
		mediumText.Add (block4);
		mediumText.Add (block5);
		mediumText.Add (block6);
		mediumText.Add (block7);
		mediumText.Add (block8);
		
		movingObs.Add (bigMushroom);
		movingObs.Add(smallMushroom);
		
		setUpFiller3 (background4.x+background4.width/2f);
	}
	
	void setUpFiller3(float startX)
	{
		FSprite background1 = new FSprite("layer1");
		Futile.stage.AddChild(background1);
		background1.SetPosition(startX + background1.width/2, background1.height/2);
		
		FSprite background2 = new FSprite("layer1");
		Futile.stage.AddChild (background2);
		background2.SetPosition(startX + background1.width + (background2.width/2)-20, background1.height/2);
		
		GSpineManager.LoadSpine("DoodleAtlas", "Atlases/DoodleJson", "Atlases/DoodleAtlas");
		Doodle canary = new Doodle("DoodleAtlas", 238f, 200f, 0.6f);
		canary.SetSkin ("Canary");
		PictureObstacle mushroom = new PictureObstacle("mushroom1");
		mushroom.scale=0.3f;
		
		MediumText blockd1 = new MediumText(blockFont, "Alice replied eagerly, for she");
		MediumText blockd2 = new MediumText(blockFont, "was always ready to talk about");
		MediumText blockd3 = new MediumText(blockFont, "her pet: 'Dinah's our cat. And");
		
		MediumText blockd4 = new MediumText(blockFont, "she's such a capital one for");
		MediumText blockd5 = new MediumText(blockFont, "catching mice you can't think!");
		MediumText blockd6 = new MediumText(blockFont, "And oh, I wish you could see");
		MediumText blockd7 = new MediumText(blockFont, "her after the birds! Why, she'll");
		
		MediumText blockd8 = new MediumText(blockFont, "eat a little bird as soon as look at it!'");
		MediumText blockd9 = new MediumText(blockFont, "This speech caused a remarkable sensation");
		
		MediumText blockd10 = new MediumText(blockFont, "among the party. Some of"); 
		MediumText blockd11 = new MediumText(blockFont, "the birds hurried off at");
		
		MediumText blockd12 = new MediumText(blockFont, "once: one old Magpie began wrapping itself up very carefully,");
		MediumText blockd13 = new MediumText(blockFont, "remarking, 'I really must be getting home; the night-air doesn't");
		
		MediumText blockd14 = new MediumText(blockFont, "suit my throat!' and a Canary");
		MediumText blockd15 = new MediumText(blockFont, "called out in a trembling voice");
		MediumText blockd16 = new MediumText(blockFont, "to its children, 'Come away, my");
		
		MediumText blockd17 = new MediumText(blockFont, "dears! It's high time you were all in bed!' On");
		MediumText blockd18 = new MediumText(blockFont, "various pretexts they all moved off, and Alice");
		
		MediumText blockd19 = new MediumText(blockFont, "was soon left alone. 'I wish");
		MediumText blockd20 = new MediumText(blockFont, "I hadn't mentioned Dinah!'");
		
		
		blockd1.SetPosition (startX + Futile.screen.width*0.01f, Futile.screen.height*0.4f);
		blockd2.SetPosition (blockd1.x, blockd1.y - blockd1.textRect.height*0.6f);
		blockd3.SetPosition (blockd1.x, blockd2.y - blockd2.textRect.height*0.6f);
		
		blockd4.SetPosition (blockd1.x + blockd4.textRect.width*0.5f, blockd1.y + blockd4.textRect.height*3f);
		blockd5.SetPosition (blockd4.x, blockd4.y - blockd5.textRect.height*0.6f);
		blockd6.SetPosition (blockd5.x, blockd5.y - blockd6.textRect.height*0.6f);
		blockd7.SetPosition (blockd6.x, blockd6.y - blockd7.textRect.height*0.6f);
		
		blockd8.SetPosition (blockd4.x + blockd8.textRect.width*0.75f, blockd4.y + blockd8.textRect.height*3.5f);
		blockd9.SetPosition (blockd8.x, blockd8.y - blockd9.textRect.height*0.6f);
		
		blockd10.SetPosition (blockd8.x + blockd10.textRect.width*0.6f, blockd9.y - blockd10.textRect.height*0.6f);
		blockd11.SetPosition (blockd10.x, blockd10.y - blockd11.textRect.height*0.6f);
		
		blockd12.SetPosition (blockd8.x + blockd12.textRect.width*0.25f, blockd8.y - blockd12.textRect.height*7f);
		blockd13.SetPosition (blockd12.x, blockd12.y - blockd13.textRect.height*0.6f);
		
		blockd14.SetPosition (blockd12.x + blockd14.textRect.width, blockd12.y + blockd14.textRect.height*3f);
		blockd15.SetPosition (blockd14.x, blockd14.y - blockd15.textRect.height*0.6f);
		blockd16.SetPosition (blockd15.x, blockd15.y - blockd16.textRect.height*0.6f);
		
		blockd17.SetPosition (blockd14.x + blockd17.textRect.width*0.5f, blockd14.y + blockd16.textRect.height*4f);
		blockd18.SetPosition (blockd17.x, blockd17.y - blockd18.textRect.height*0.6f);
		
		blockd19.SetPosition (blockd17.x + blockd19.textRect.width*0.6f, blockd17.y - blockd19.textRect.height*3f);
		blockd20.SetPosition (blockd19.x, blockd19.y - blockd20.textRect.height*0.6f);
		
		canary.SetPosition (blockd14.x, blockd14.y+canary.height/2f);
		mushroom.SetPosition(blockd13.x + blockd13.textRect.width/2f, groundHeight+mushroom.height/2.3f);
		
		Futile.stage.AddChild (canary);
		Futile.stage.AddChild (mushroom);
		canary.Start ();
		girl.addDoodle (canary);
		
		mediumText.Add (blockd1);
		mediumText.Add (blockd2);
		mediumText.Add (blockd3);
		mediumText.Add (blockd4);
		mediumText.Add (blockd5);
		mediumText.Add (blockd6);
		mediumText.Add (blockd7);
		mediumText.Add (blockd8);
		mediumText.Add (blockd9);
		mediumText.Add (blockd10);
		mediumText.Add (blockd11);
		mediumText.Add (blockd12);
		mediumText.Add (blockd13);
		mediumText.Add (blockd14);
		mediumText.Add (blockd15);
		mediumText.Add (blockd16);
		mediumText.Add (blockd17);
		mediumText.Add (blockd18);
		mediumText.Add (blockd19);
		mediumText.Add (blockd20);
		
		
		setUpEggStage (background2.x+background2.width/2f);
	}
	
	void setUpEggStage(float startX)
	{
		FSprite background = new FSprite("layer1");
		Futile.stage.AddChild (background);
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("layer1");
		Futile.stage.AddChild (background2);
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("layer1");
		Futile.stage.AddChild (background3);
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("layer1");
		Futile.stage.AddChild (background4);
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
		
		FSprite background5 = new FSprite("layer1");
		Futile.stage.AddChild (background5);
		background5.SetPosition((background.width/2)-20 + background4.x, background.height/2);
		
		
		GSpineManager.LoadSpine("EggAtlas", "Atlases/EggJson", "Atlases/EggAtlas");
		CrackingEggs egg1 = new CrackingEggs("EggAtlas", 0.3f, girl);
		egg1.scale = 0.3f;
		
		CrackingEggs egg2 = new CrackingEggs("EggAtlas", 0.3f, girl);
		egg2.scale = 0.3f;
		
		CrackingEggs egg3 = new CrackingEggs("EggAtlas", 0.3f, girl);
		egg3.scale = 0.3f;
		
		CrackingEggs bigEgg = new CrackingEggs("EggAtlas", 0.6f, girl);
		bigEgg.scale = 0.6f;
		
		MediumText sentence1 = new MediumText(blockFont, "'As if I wasn't having enough trouble");
		MediumText sentence2 = new MediumText(blockFont, "the eggs'");
		
		MediumText block1 = new MediumText(blockFont, "Alice was more and more");
		MediumText block2 = new MediumText(blockFont, "puzzled,  but she thought there was no use");
		MediumText block3 = new MediumText (blockFont, "in saying anything more till the Pigeon had finished.");
		
		AffectPictureWords hatching = new AffectPictureWords(specialFont, "hatching", 1f, bigEgg);
		
		specialWords.Add (hatching);
		
		
		block3.SetPosition (startX + block3.textRect.width/2f,groundHeight + block3.textRect.height);
		block2.SetPosition(block3.x, block3.y + block2.textRect.height);
		block1.SetPosition (block3.x, block2.y + block3.textRect.height);
		
		sentence1.SetPosition(block3.x + block3.textRect.width/1.5f, block1.y + sentence1.textRect.height);
		
		egg1.SetPosition (sentence1.x + sentence1.textRect.width/1.5f, groundHeight);
		egg2.SetPosition (egg1.x + egg2.width, groundHeight);
		egg3.SetPosition (egg2.x + egg3.width, groundHeight);
		hatching.SetPosition (egg2.x, groundHeight+ egg2.height/2f);
		
		sentence2.SetPosition(egg3.x + egg3.width/1.5f, groundHeight + sentence2.textRect.height);
		bigEgg.SetPosition (sentence2.x + bigEgg.width/1.5f, groundHeight-sentence2.textRect.height);
		
		egg1.Start ();
		egg2.Start ();
		egg3.Start ();
		bigEgg.Start ();
		
		mediumText.Add (sentence1);
		mediumText.Add (sentence2);
		
		mediumText.Add (block1);
		mediumText.Add (block2);
		mediumText.Add (block3);
		
		movingObs.Add (egg1);
		movingObs.Add (egg2);
		movingObs.Add (egg3);
		movingObs.Add (bigEgg);
		
		updateObs.Add (egg1);
		updateObs.Add (egg2);
		updateObs.Add (egg3);
		updateObs.Add (bigEgg);
		
		setUpFiller4 (background5.x+background5.width/2f);
		
	}

	void setUpFiller4(float startX)
	{
		//'Oh dear, what nonsense I'm ... along in a great hurry, muttering
		FSprite background1 = new FSprite("layer1");
		Futile.stage.AddChild(background1);
		background1.SetPosition(startX + background1.width/2, background1.height/2);
		
		FSprite background2 = new FSprite("layer1");
		Futile.stage.AddChild (background2);
		background2.SetPosition(startX + background1.width + (background2.width/2)-20, background1.height/2);		
		
		FSprite background3 = new FSprite("layer1");
		Futile.stage.AddChild (background3);
		background3.SetPosition(startX + background1.width + background2.width + (background3.width/2)-20, background1.height/2);
		
		MediumText blockd1 = new MediumText(blockFont, "'I HAVE tasted eggs, certainly,' said");
		MediumText blockd2 = new MediumText(blockFont, "Alice, who was a very truthful child;");
		
		MediumText blockd3 = new MediumText(blockFont, "'but little girls eat eggs quite as");
		MediumText blockd4 = new MediumText(blockFont, "much as serpents do, you know.' 'I");
		
		MediumText blockd5 = new MediumText(blockFont, "don't believe it,' said the Pigeon;");
		MediumText blockd6 = new MediumText(blockFont, "'but if they do, why then they're a");
		
		MediumText blockd7 = new MediumText(blockFont, "kind of serpent,");
		MediumText blockd8 = new MediumText(blockFont, "that's all I can");
		MediumText blockd9 = new MediumText(blockFont, "say.' This was such");
		
		MediumText blockd10 = new MediumText(blockFont, "a new idea to Alice, that"); 
		
		MediumText blockd11 = new MediumText(blockFont, "she was quite silent for a minute or two,");
		MediumText blockd12 = new MediumText(blockFont, "which gave the Pigeon the opportunity of");
		
		MediumText blockd13 = new MediumText(blockFont, "adding, 'You're looking for eggs, I know THAT well enough;");
		MediumText blockd14 = new MediumText(blockFont, "and what does it matter to me whether you're a little girl");
		
		MediumText blockd15 = new MediumText(blockFont, "or a serpent?' 'It");
		MediumText blockd16 = new MediumText(blockFont, "matters a good deal");
		MediumText blockd17 = new MediumText(blockFont, "to ME,' said Alice hastily");
		
		MediumText blockd18 = new MediumText(blockFont, "'but I'm not looking for eggs, as it happens; and if");
		MediumText blockd19 = new MediumText(blockFont, "I was, I shouldn't want YOURS: I don't like them raw.'");
		
		
		blockd1.SetPosition (startX, Futile.screen.height*0.3f);
		blockd2.SetPosition (blockd1.x, blockd1.y - blockd1.textRect.height*0.6f);
		
		blockd3.SetPosition (blockd1.x + blockd3.textRect.width*0.5f, blockd1.y + blockd3.textRect.height*2.5f);
		blockd4.SetPosition (blockd3.x, blockd3.y - blockd4.textRect.height*0.6f);
		
		blockd5.SetPosition (blockd3.x + blockd5.textRect.width*0.4f, blockd3.y + blockd5.textRect.height*2.5f);
		blockd6.SetPosition (blockd5.x, blockd5.y - blockd6.textRect.height*0.6f);
		
		blockd7.SetPosition (blockd5.x + blockd7.textRect.width*1.75f, blockd5.y);
		blockd8.SetPosition (blockd7.x, blockd7.y - blockd8.textRect.height*0.6f);
		blockd9.SetPosition (blockd8.x, blockd8.y - blockd9.textRect.height*0.6f);
		
		blockd10.SetPosition (blockd7.x + blockd10.textRect.width*0.75f, blockd7.y + blockd10.textRect.height*3f);
		
		blockd11.SetPosition (blockd10.x + blockd11.textRect.width*0.75f, blockd10.y + blockd11.textRect.height*0.6f);
		blockd12.SetPosition (blockd11.x, blockd11.y - blockd12.textRect.height*0.6f);
		
		blockd13.SetPosition (blockd7.x + blockd13.textRect.width*0.4f, blockd7.y - blockd13.textRect.height*3.5f);
		blockd14.SetPosition (blockd13.x, blockd13.y - blockd14.textRect.height*0.6f);
		
		blockd15.SetPosition (blockd13.x + blockd15.textRect.width*2f, blockd13.y - blockd15.textRect.height);
		blockd16.SetPosition (blockd15.x, blockd15.y - blockd16.textRect.height*0.6f);
		blockd17.SetPosition (blockd16.x, blockd16.y - blockd17.textRect.height*0.6f);
		
		blockd18.SetPosition (blockd15.x + blockd18.textRect.width*0.5f, blockd15.y + blockd18.textRect.height*3f);
		blockd19.SetPosition (blockd18.x, blockd18.y - blockd19.textRect.height*0.6f);
		
		
		mediumText.Add (blockd1);
		mediumText.Add (blockd2);
		mediumText.Add (blockd3);
		mediumText.Add (blockd4);
		mediumText.Add (blockd5);
		mediumText.Add (blockd6);
		mediumText.Add (blockd7);
		mediumText.Add (blockd8);
		mediumText.Add (blockd9);
		mediumText.Add (blockd10);
		mediumText.Add (blockd11);
		mediumText.Add (blockd12);
		mediumText.Add (blockd13);
		mediumText.Add (blockd14);
		mediumText.Add (blockd15);
		mediumText.Add (blockd16);
		mediumText.Add (blockd17);
		mediumText.Add (blockd18);
		mediumText.Add (blockd19);

		
		setUpTwinkleStage (background3.x+background3.width/2f);
	}
	
	void setUpTwinkleStage(float startX)
	{
		FSprite background = new FSprite("layer1");
		Futile.stage.AddChild (background);
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("layer1");
		Futile.stage.AddChild (background2);
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("layer1");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("layer1");
		Futile.stage.AddChild (background4);
		background4.scale = 0.6f;
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
	
		MediumText block1 = new MediumText(blockFont, "\"I don't think...\"");
		MediumText block2 = new MediumText(blockFont, "\"Then you you shouldn't");
		MediumText block3 = new MediumText(blockFont, ",\"said the Hatter.");
		
		MediumText twinkle1 = new MediumText(blockFont, "Twinkle twinkle little bat");
		MediumText twinkle2 = new MediumText(blockFont, "Up above the world you fly");
		MediumText twinkle3 = new MediumText(blockFont, "like a tea tray in the sky");
		MediumText twinkle4 = new MediumText(blockFont, "how I wonder what you're at");
		
		MediumText block4 = new MediumText(blockFont, "Here the Dormouse shook itself , and began singing\n\n" +
													  "in its sleep\'Twinkle, twinkle, twinkle, twinkle—\'\n\n" +
													  "and went on so long that they had to pinch it to ");
		MediumText block5 = new MediumText(blockFont, "make it stop.\'Well, I'd hardly finished the first\n\n" +
												   	  "verse,' said the Hatter, \'when the Queen jumped up\n\n " +
												   	  "and bawled out, \"He's murdering the time! Off with\n\n" +
												   	  "his head!\"\'\'How dreadfully savage!' exclaimed Alice.\n\n");
		MediumText block6 = new MediumText(blockFont, "\'And ever since that,\'the Hatter went on in a mournful\n\n" +
												      "tone, \'he won't do a thing I ask! It's always six o\'\n\n" +
												      "clock now.\'A bright idea came into Alice's head.\'Is that\n\n" +
												      "the reason so many tea-things are put out here?\' she asked.");
		
		
		mediumText.Add (block1);
		mediumText.Add (block2);
		mediumText.Add (block3);
		mediumText.Add (block4);
		mediumText.Add (block5);
		mediumText.Add (block6);
		
		mediumText.Add (twinkle1);
		mediumText.Add (twinkle2);
		mediumText.Add (twinkle3);
		mediumText.Add (twinkle4);
		
		twinkleText.Add (twinkle1);
		twinkleText.Add (twinkle2);
		twinkleText.Add (twinkle3);
		twinkleText.Add (twinkle4);
		
	
		foreach (MediumText txt in twinkleText)
		{
			txt.alpha=0;
			txt.setSolidity (false);
		}
		
		AffectPictureWords talk = new AffectPictureWords(specialFont, "talk", 1f, twinkleText, 2);
		
		specialWords.Add (talk);
		
		block1.SetPosition (startX, groundHeight + block1.textRect.height*2f);
		block2.SetPosition (block1.x+block2.textRect.width/2f, block1.y + block2.textRect.height*3f);
		talk.SetPosition (block2.x + block2.textRect.width/2.5f, block2.y);
		block3.SetPosition (block2.x, block2.y-block3.textRect.height/2f);
	
		twinkle1.SetPosition (talk.x + twinkle1.textRect.width/2.2f, talk.y+twinkle1.textRect.height*2f);
		twinkle2.SetPosition (twinkle1.x + twinkle2.textRect.width/1.5f, block3.y);
		twinkle3.SetPosition (twinkle2.x + twinkle3.textRect.width/1.5f, twinkle1.y+twinkle3.textRect.height);
		twinkle4.SetPosition (twinkle3.x + twinkle4.textRect.width/1.5f, twinkle3.y+twinkle4.textRect.height*3f);
		
		block4.SetPosition (twinkle4.x+block4.textRect.width/1.5f, twinkle4.y - (block4.textRect.height-twinkle4.textRect.height)/4f);
		block5.SetPosition (block4.x, block4.y-block5.textRect.height/1.9f);
		block6.SetPosition (block4.x, block5.y-block6.textRect.height/1.7f);
		
		setUpTrumpetStage (background4.x+background4.width/2f);
	}
	
	void setUpTrumpetStage(float startX)
	{
		FSprite background = new FSprite("layer1");
		Futile.stage.AddChild (background);
		background.scale = 0.6f;
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("layer1");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("layer1");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("layer1");
		Futile.stage.AddChild (background4);
		background4.scale = 0.6f;
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
		
		GSpineManager.LoadSpine ("ScrollAtlas", "Atlases/ScrollJson", "Atlases/ScrollAtlas");
		Scroll scroll = new Scroll("ScrollAtlas", 0.6f);
		scroll.scale=0.6f;
		
		GSpineManager.LoadSpine("TrumpetAtlas", "Atlases/TrumpetJson", "Atlases/TrumpetAtlas");
		Trumpet trumpet = new Trumpet("TrumpetAtlas", 0.6f, scroll);
		trumpet.scale = 0.6f;
		
		MediumText block1 = new MediumText(blockFont, "'Herald, read the accusation!' said the King.'");

		MediumText block2 = new MediumText(blockFont, "'Consider your verdict,' the King said to the\n" +
														"\njury. ' Not yet,not yet!' the Rabbit hastily\n" +
													 "\ninterrupted.'There's a great deal to come before\n" +
													 "\nthat!' 'Call the first witness,' said the King\n");
		MediumText block3 = new MediumText(blockFont, "and the White Rabbit blew three  blasts on the \n\n" +
													  "trumpet, and called out, 'First witness!' The first\n\n" +
													  "witness was the Hatter. He came in with a teacup in \n\n" +
													  "one hand and a piece of bread-and-butter in the other. '");
												
		
		MediumText sentence1 = new MediumText(blockFont, "On this the White Rabbit");
		MediumText sentence2 = new MediumText(blockFont, " three blasts on");
		MediumText sentence3 = new MediumText(blockFont, "the trumpet, and then unrolled the parchment \n\n" +
														"scroll, and read as follows:");
		
		mediumText.Add (sentence1);
		mediumText.Add (sentence2);
		mediumText.Add (sentence3);
		
		mediumText.Add (block1);
		mediumText.Add (block2);
		mediumText.Add(block3);
		
		AffectPictureWords blew = new AffectPictureWords(specialFont, "blew", 1f, trumpet);
		
		block1.SetPosition (startX + block1.textRect.width/2f, groundHeight + block1.textRect.height*3f);
		sentence3.SetPosition (block1.x + sentence3.textRect.width/3f, block1.y + sentence3.textRect.height/2f);
		sentence1.SetPosition (sentence3.x-(sentence3.textRect.width-sentence1.textRect.width)/4f, sentence3.y + sentence1.textRect.height*2f);		
		blew.SetPosition (sentence1.x + blew.textRect.width*2f, sentence1.y);
		sentence2.SetPosition (blew.x + sentence2.textRect.width/2f, sentence1.y);
	
		Futile.stage.AddChild (blew);
		specialWords.Add (blew);
		
		trumpet.SetPosition (blew.x, groundHeight);
		scroll.SetPosition (trumpet.x + scroll.width/1.5f, Futile.screen.height*0.2f);
		trumpet.Start();
		scroll.Start();
		
		movingObs.Add (scroll);
		
		Futile.stage.AddChild (trumpet);
		
		block3.SetPosition (scroll.x + block2.textRect.width/1.5f, Futile.screen.height*0.3f);
		block2.SetPosition (block3.x, block3.y + block2.textRect.height/1.5f);
		
		setUpMalletStage (background4.x+background4.width/2f);
	}
	
	void setUpMalletStage(float startX)
	{
		
		FSprite background = new FSprite("layer1");
		Futile.stage.AddChild (background);
		background.scale = 0.6f;
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("layer1");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("layer1");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("layer1");
		Futile.stage.AddChild (background4);
		background4.scale = 0.6f;
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
	
		GSpineManager.LoadSpine ("WaterGlassAtlas", "Atlases/WaterGlassJson", "Atlases/WaterGlassAtlas");
		WaterGlass glass = new WaterGlass("WaterGlassAtlas", 0.6f, groundHeight);
		glass.scale=0.35f;

		GSpineManager.LoadSpine("MalletAtlas", "Atlases/MalletJson", "Atlases/MalletAtlas");
		Mallet mallet = new Mallet("MalletAtlas", 0.6f);
		mallet.scale = 0.6f;

		GSpineManager.LoadSpine ("LighteningAtlas", "Atlases/LighteningJson", "Atlases/LighteningAtlas");
		Lightening lightening = new Lightening("LighteningAtlas", 0.6f);
		lightening.scale = 0.4f;
		
		AffectPictureWords tremble = new AffectPictureWords(specialFont, "tremble", 1f, glass);
		AffectPictureWords thunderstorm = new AffectPictureWords(specialFont, "thunderstorm", 1f, lightening);
		
		
		MediumText block1 = new MediumText(blockFont, "But here, the Duchess's voice died away, and hers arm began to ");
		MediumText block2 = new MediumText(blockFont, "Alice looked up, and there stood the Queen, frowning like a ");

		block1.SetPosition (startX + block1.textRect.width/1.5f, groundHeight + girl.girlHeight*2f);
		block2.SetPosition (startX + block2.textRect.width/2f, lightening.lighteningHeight - block2.textRect.height);

		block1.SetPosition (startX + block1.textRect.width/1.5f, groundHeight + girl.girlHeight*2f);
		block2.SetPosition (startX + block2.textRect.width/2f, lightening.lighteningHeight);
		tremble.SetPosition(block1.x+(block1.textRect.width/2f)*mediumTextScale+tremble.rect.width/2f, block1.y);
		thunderstorm.SetPosition (block2.x + block2.textRect.width/2.4f, block2.y);
		
		mediumText.Add (block1);
		mediumText.Add (block2);
		
		specialWords.Add (tremble);
		specialWords.Add (thunderstorm);
		
		lightening.Start ();
		lightening.addCollider (glass);
		glass.addCollider (mallet);
		mallet.Start ();
		
		mallet.SetPosition (tremble.x+mallet.height/1.5f, groundHeight);
		glass.SetPosition (tremble.x, tremble.y+tremble.textRect.height/2f);
		lightening.SetPosition (thunderstorm.x, thunderstorm.y);
		
		mallet.makeRect ();
		glass.Start ();
		updateObs.Add(glass);
		
		Futile.stage.AddChild (lightening);
		
		movingObs.Add (glass);
		movingObs.Add (mallet);
		
		foreach (MediumText mt in mediumText)
		{
			Futile.stage.AddChild (mt);
			mt.scale = mediumTextScale;
		}
				
		foreach (SpecialWords sw in specialWords)
		{
			Futile.stage.AddChild (sw);
		}
		
		foreach(MovingPictureObstacles pic in movingObs)
		{
			Futile.stage.AddChild(pic);
		}
	}
	
	
	Rect makeTextRect(FLabel l)
	{
		Rect r;
		r = new Rect(l.x - 0.6f * l.textRect.width/2, l.y, 0.6f * l.textRect.width, 0.6f * l.textRect.height);
		return r;
	}
	
	FSprite makeBackground(float x)
	{
		FSprite background = new FSprite("layer1");
		background.SetPosition(x, Futile.screen.halfHeight);
		Futile.stage.AddChild(background);
		
		FSprite layer = new FSprite("layer2");
		layer.SetPosition(x, Futile.screen.halfHeight);
		Futile.stage.AddChild(layer);
		layer2.Add(layer);
		
		layer = new FSprite("layer3");
		layer.SetPosition(x, Futile.screen.halfHeight);
		Futile.stage.AddChild(layer);
		layer3.Add(layer);
		
		return background;
	}
	
	FSprite makeBackground()
	{
		FSprite background = new FSprite("layer1");
		background.SetPosition(background.width/2, Futile.screen.halfHeight);
		Futile.stage.AddChild(background);
		
		FSprite layer = new FSprite("layer2");
		layer.SetPosition(background.width/2, Futile.screen.halfHeight);
		Futile.stage.AddChild(layer);
		layer2.Add(layer);
		
		layer = new FSprite("layer3");
		layer.SetPosition(background.width/2, Futile.screen.halfHeight);
		Futile.stage.AddChild(layer);
		layer3.Add(layer);
		
		return background;
	}
	
	void updateBackground()
	{
		foreach (FSprite l in layer2)
		{
			l.x -= layer2speed;	
		}
		
		foreach (FSprite l in layer3)
		{
			l.x -= layer3speed;	
		}
	}
	
}
