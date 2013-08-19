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
	List <MediumText> twinkleText = new List<MediumText>();
	
	bool directionTouchFound;
	bool inSpecialWord=false;
	
	List <SpecialWords> specialWords = new List<SpecialWords> ();
	List <MediumText> mediumText = new List<MediumText>();
	List <PictureObstacle> pictures = new List<PictureObstacle>();
	
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
		blockFont = "PalatinoMedium";
		specialFont = "PalatinoSpecial";
		
		//load sprites
		background = new FSprite("AliceBG1");
		ground = new Ground(groundHeight, "PalatinoMedium");
		//power1 = new FLabel("PalitinoMedium", "JUMP!");
	}
	
	void SetUpStage()
	{
		setUpTutorialStage();
		//girl
		
		Futile.stage.AddChild(girl);
		girl.SetPosition(0, groundHeight);
		girl.setGroundHeight (groundHeight);
		
		//ground text
		ground.Start();
		
		girl.Start();
		
		
		// makes it so that the entire screen is capable of multitouch
		Futile.touchManager.AddMultiTouchTarget (this);
		
		//set camera to follow girl
		cam.setWorldBounds(new Rect(-1.5f * Futile.screen.width, -.5f*Futile.screen.height, 20*Futile.screen.width, 1.25f* Futile.screen.height));
		cam.follow(focus);
		
		
	}
	
	void Update()
	{
		girl.Update();
		bool solid;
		
		girl.isGrounded = false;
		
		foreach (Rect text in textRects)
		{
			//girl.checkCollisions(text);
		}
		
		for(int x = pictures.Count-1; x>=0; x--)
		{
			//solid = pictures[x].isSolid ();
			//girl.checkCollisions (pictureObstacleRects[x], solid);
		}
		
		for(int x = specialWords.Count-1; x>=0; x--)
		{
			//solid = specialWords[x].isSolid ();
			//girl.checkCollisions (specialWordRects[x], solid);
		}
		
		for(int x = mediumText.Count-1; x>=0; x--)
		{
			solid = mediumText[x].isSolid ();
			girl.checkCollisions (mediumTextRects[x], solid);
		}
		
		foreach (MediumText txt in twinkleText)
		{
			txt.Update();
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
		Futile.stage.AddChild(background);
		background.scale = 0.6f;
		background.SetPosition(background.width/2, background.height/2);
		FSprite background2 = new FSprite("blank");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition(background.width + (background2.width/2)-20, background.height/2);
		
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
		pictures.Add (hole);
		
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
		blocka9.SetPosition (blocka1.x + (blocka9.textRect.width - blocka1.textRect.width)/4f, blocka8.y - blocka9.textRect.height);
		blocka10.SetPosition (blocka9.x, blocka9.y - blocka10.textRect.height);
		blocka11.SetPosition (blocka9.x, blocka10.y - blocka11.textRect.height);
		
		blockb.SetPosition (blocka11.x - blockb.textRect.width/2f, groundHeight+(blocka11.y-groundHeight)/2f);
		
		blockc5.SetPosition(blocka1.x + blockc5.textRect.width/2f, blocka1.y + blockc5.textRect.height/2f);
		blockc6.SetPosition(blockc5.x + blockc6.textRect.width*2.5f, blockc5.y);
		blockc3.SetPosition (blockc5.x, blockc5.y + blockc3.textRect.height);
		blockc4.SetPosition (blockc6.x, blockc6.y+blockc4.textRect.height);
		blockc1.SetPosition (blockc5.x+(blockc1.textRect.width/2f - blockc3.textRect.width/2f), blockc1.textRect.height + blockc3.y);
		blockc2.SetPosition (blockc6.x - + (blockc2.textRect.width/2f - blockc4.textRect.width/2f), blockc2.textRect.height + blockc4.y);
		
		sentence1.SetPosition (blocka7.x + sentence1.textRect.width/1.5f, blocka9.y+sentence1.textRect.height/1.5f);
		
		ChangeWord down = new ChangeWord(specialFont, "down");
		Futile.stage.AddChild(down);
		down.SetPosition (sentence1.x + down.textRect.width*1.5f, sentence1.y);
		
		sentence2.SetPosition(down.x+sentence2.textRect.width/2.5f, down.y);
		sentence3.SetPosition (sentence2.x+(sentence2.textRect.width-sentence3.textRect.width)/4f, sentence2.y-sentence3.textRect.height);
		sentence4.SetPosition(sentence3.x, sentence3.y-sentence4.textRect.height);
		
		
		hole.SetPosition ((blockc1.x+blockc2.x)/2f, blockc1.y);
		
		float outX = sentence4.x+girl.girlWidth*4f;
		float outY = girl.y;
		Vector2 outPosition = new Vector2(outX, outY);
		ChangeGirlPositionWord Out = new ChangeGirlPositionWord(specialFont, "out", outPosition, girl);
		Futile.stage.AddChild (Out);
		Out.SetPosition(sentence4.x, groundHeight + (sentence4.y-groundHeight)/2f);
		
		specialWords.Add (Out);
		specialWords.Add (down);
		
		setUpTwinkleStage (background2.x + background2.width/2f);
		
	}
	
	void setUpShrinkStage(float startX)
	{
		FSprite background3 = new FSprite("blank");
		FSprite background4 = new FSprite("blank");

		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background3.width/2)-20 + startX, background.height/2);
		Futile.stage.AddChild (background4);
		background4.scale = 0.6f;
		background4.SetPosition(background3.x +(background3.width/2)-20, background.height/2);
	
		ChangeGirlSizeWord shrink = new ChangeGirlSizeWord(specialFont, "shrink", 0.3f, girl);
		ChangeGirlSizeWord grow = new ChangeGirlSizeWord(specialFont, "grow", 0.6f, girl);
		
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
		
		sentence1.SetPosition (startX + sentence1.textRect.width/2f, Futile.screen.height*0.85f);
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
		MediumText sentence5 = new MediumText(blockFont, "\"and it if makes me ");
		MediumText sentence6 = new MediumText(blockFont, " larger, I can reach the key.\"");
		
		mediumText.Add (sentence4);
		mediumText.Add (sentence5);
		mediumText.Add (sentence6);
		
		sentence4.SetPosition (blocka1.x + sentence4.textRect.width, blocka2.y);
		sentence5.SetPosition (sentence4.x, sentence4.y - sentence5.textRect.height);
		grow.SetPosition (sentence5.x - (sentence5.textRect.width - grow.textRect.width)/4f, sentence5.y - grow.textRect.height);
		sentence6.SetAnchor (grow.x + sentence6.textRect.width/2f, grow.y);
		
		setUpEggStage (background4.x + background4.width/2f);
		
	}
	
	void setUpMushroomStage(float startX)
	{
		FSprite background = new FSprite("blank");
		Futile.stage.AddChild (background);
		background.scale = 0.6f;
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("blank");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("blank");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("blank");
		Futile.stage.AddChild (background4);
		background4.scale = 0.6f;
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
		
		GSpineManager.LoadSpine("MushroomAtlas", "Atlases/MushroomJson", "Atlases/MushroomAtlas");
		GrowingMushroom smallMushroom = new GrowingMushroom("MushroomAtlas", "small", 0.6f);
		smallMushroom.scale = 0.6f;
		smallMushroom.Start();
		
		GrowingMushroom bigMushroom = new GrowingMushroom("MushroomAtlas", "big", 0.6f);
		bigMushroom.scale = 0.6f;
		bigMushroom.Start ();
		
		Futile.stage.AddChild (smallMushroom);
		Futile.stage.AddChild(bigMushroom);
		
		AffectPictureWords bgrow = new AffectPictureWords(specialFont, "grow", smallMushroom);
		bgrow.SetPosition (300, groundHeight + 20f);
		specialWords.Add (bgrow);
		
		AffectPictureWords sgrow = new AffectPictureWords(specialFont, "grow", bigMushroom);
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
		
		sentence1.SetPosition(startX, Futile.screen.height*0.4f);
		bgrow.SetPosition (sentence1.x + sentence1.textRect.width/2.3f, sentence1.y);
		sentence2.SetPosition (sentence1.x + (sentence2.textRect.width-sentence1.textRect.width)/4f, sentence1.y - sentence2.textRect.height);
		sgrow.SetPosition (sentence2.x + sentence2.textRect.width/2.6f, sentence2.y);
		sentence3.SetPosition (sgrow.x+sentence3.textRect.width/1.7f, sentence2.y);
		
		bigMushroom.SetPosition (sentence3.x + sentence3.textRect.width*1.5f, groundHeight);
		smallMushroom.SetPosition (bigMushroom.x + smallMushroom.width*1.5f, groundHeight);
		
		block1.SetPosition (smallMushroom.x + block1.textRect.width/2f, bigMushroom.height);
		block2.SetPosition (block1.x - (block1.textRect.width-block2.textRect.width)/4f, block1.y-block2.textRect.height);
		block3.SetPosition (block2.x, block2.y-block3.textRect.height);
		block4.SetPosition (block2.x, block3.y-block4.textRect.height);
		block5.SetPosition (block2.x, block4.y-block5.textRect.height);
		block6.SetPosition (block2.x, block5.y-block6.textRect.height);
		block7.SetPosition (block2.x, block6.y-block7.textRect.height);
		
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
		
				
		foreach (MediumText mt in mediumText)
		{
			Futile.stage.AddChild (mt);
			mt.scale = 0.6f;
			Rect mtRect = makeTextRect(mt);
			mediumTextRects.Add (mtRect);
		}
				
		foreach (SpecialWords sw in specialWords)
		{
			Futile.stage.AddChild (sw);
			Rect swRect = makeTextRect(sw);
			specialWordRects.Add (swRect);
		}
		
		foreach (PictureObstacle pic in pictures)
		{
			Rect picRect = pic.localRect.CloneAndOffset(pic.x, pic.y);
			pictureObstacleRects.Add(picRect);
		}
		
		
	}
	
	void setUpEggStage(float startX)
	{
		FSprite background = new FSprite("blank");
		Futile.stage.AddChild (background);
		background.scale = 0.6f;
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("blank");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("blank");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("blank");
		Futile.stage.AddChild (background4);
		background4.scale = 0.6f;
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
		
		GSpineManager.LoadSpine("EggAtlas", "Atlases/EggJson", "Atlases/EggAtlas");
		CrackingEggs egg1 = new CrackingEggs("EggAtlas", 0.3f);
		egg1.scale = 0.3f;
		egg1.Start();
		
		CrackingEggs egg2 = new CrackingEggs("EggAtlas", 0.3f);
		egg2.scale = 0.3f;
		egg2.Start();
		
		CrackingEggs egg3 = new CrackingEggs("EggAtlas", 0.3f);
		egg3.scale = 0.3f;
		egg3.Start();
		
		CrackingEggs bigEgg = new CrackingEggs("EggAtlas", 0.6f);
		bigEgg.scale = 0.6f;
		bigEgg.Start ();
		
		MediumText sentence1 = new MediumText(blockFont, "'As if I wasn't having enough trouble");
		MediumText sentence2 = new MediumText(blockFont, "the eggs'");
		
		MediumText block1 = new MediumText(blockFont, "Alice was more and more");
		MediumText block2 = new MediumText(blockFont, "puzzled,  but she thought there was no use");
		MediumText block3 = new MediumText (blockFont, "in saying anything more till the Pigeon had finished.");
		
		AffectPictureWords hatching = new AffectPictureWords(specialFont, "hatching", bigEgg);
		
		specialWords.Add (hatching);
		
		
		block3.SetPosition (startX + block3.textRect.width/2f,groundHeight + block3.textRect.height);
		block2.SetPosition(block3.x, block3.y + block2.textRect.height);
		block1.SetPosition (block3.x, block2.y + block3.textRect.height);
		
		sentence1.SetPosition(block3.x + block3.textRect.width/1.5f, block1.y + sentence1.textRect.height);
		
		egg1.SetPosition (sentence1.x + sentence1.textRect.width/1.5f, groundHeight);
		egg2.SetPosition (egg1.x + egg2.width, groundHeight);
		egg3.SetPosition (egg2.x + egg3.width, groundHeight);
		hatching.SetPosition (egg2.x, groundHeight+ egg2.height + hatching.textRect.height);
		
		sentence2.SetPosition(egg3.x + egg3.width/1.5f, groundHeight + sentence2.textRect.height);
		bigEgg.SetPosition (sentence2.x + bigEgg.width/1.5f, groundHeight-sentence2.textRect.height);
		
		Futile.stage.AddChild(egg1);
		Futile.stage.AddChild(egg2);
		Futile.stage.AddChild(egg3);
		Futile.stage.AddChild(bigEgg);
		
		mediumText.Add (sentence1);
		mediumText.Add (sentence2);
		
		mediumText.Add (block1);
		mediumText.Add (block2);
		mediumText.Add (block3);
		
	}
	
	void setUpRotationStage(float startX)
	{
		FSprite background = new FSprite("blank");
		Futile.stage.AddChild (background);
		background.scale = 0.6f;
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("blank");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("blank");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("blank");
		Futile.stage.AddChild (background4);
		background4.scale = 0.6f;
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
		
		MediumText sentence1 = new MediumText(blockFont, "So they began solemnly dancing round and round");
		MediumText sentence2 = new MediumText(blockFont, "Alice, every now and then treading on her toes");
		MediumText sentence3 = new MediumText(blockFont, "when they passed too close , and  waving their");
		MediumText sentence4 = new MediumText(blockFont, "forepaws to mark the time while the Mock Turtle");
		MediumText sentence5 = new MediumText(blockFont, "sang this, very slowly and sadly:—");
		
		mediumText.Add (sentence1);
		mediumText.Add (sentence2);
		mediumText.Add (sentence3);
		mediumText.Add (sentence4);
		mediumText.Add (sentence5);
		
		MediumText rotateBlock1 = new MediumText(blockFont, "\"'Will you walk a little faster?\" said a whiting to a snail.\n\"There's a porpoise close behind us, and he's treading on my tail.");
		MediumText rotateBlock2 = new MediumText( blockFont, "See how eagerly the lobsters and the turtles all advance!\nThey are waiting on the shingle—will you come and join the dance?");
		
		mediumText.Add (rotateBlock1);
		mediumText.Add (rotateBlock2);
		
		Vector2 rotatePoint1 = new Vector2(rotateBlock1.x, rotateBlock1.y);
		Vector2 rotatePoint2 = new Vector2(rotateBlock2.x, rotateBlock2.y);
		rotateBlock1.RotateAroundPointAbsolute (rotatePoint1,90f);
		rotateBlock2.RotateAroundPointAbsolute( rotatePoint2, 90f);;
		
		AffectPictureWords round1 = new AffectPictureWords(specialFont, "round", rotateBlock1,1);
		AffectPictureWords round2 = new AffectPictureWords(specialFont, "round", rotateBlock2, 1);
		
		round1.SetPosition (startX + round1.textRect.width, groundHeight*2);
		round2.SetPosition (round1.x, groundHeight*1.5f);
		round2.addCollisionObjects (round1);
		
		rotateBlock1.SetPosition (round1.x + rotateBlock1.textRect.width/1.5f, groundHeight + rotateBlock1.textRect.width/2f);
		rotateBlock2.SetPosition (rotateBlock1.x + rotateBlock2.textRect.width/3f, groundHeight + rotateBlock2.textRect.width/1.5f);
	}
	
	void setUpTrumpetStage(float startX)
	{
		FSprite background = new FSprite("blank");
		Futile.stage.AddChild (background);
		background.scale = 0.6f;
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("blank");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("blank");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("blank");
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
		Debug.Log("2");
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
		
		AffectPictureWords blew = new AffectPictureWords(specialFont, "blew", trumpet);
		
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
		
		block3.SetPosition (scroll.x + block2.textRect.width/1.5f, Futile.screen.height*0.3f);
		block2.SetPosition (block3.x, block3.y + block2.textRect.height/1.5f);
		
	}
	
	void setUpMalletStage(float startX)
	{
		FSprite background = new FSprite("blank");
		Futile.stage.AddChild (background);
		background.scale = 0.6f;
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("blank");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("blank");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("blank");
		Futile.stage.AddChild (background4);
		background4.scale = 0.6f;
		background4.SetPosition((background.width/2)-20 + background3.x, background.height/2);
	
		GSpineManager.LoadSpine ("WaterGlassAtlas", "Atlases/WaterGlassJson", "Atlases/WaterGlassAtlas");
		WaterGlass glass = new WaterGlass("WaterGlassAtlas", 0.6f, groundHeight);
		glass.scale=0.6f;

		GSpineManager.LoadSpine("MalletAtlas", "Atlases/MalletJson", "Atlases/MalletAtlas");
		Mallet mallet = new Mallet("MalletAtlas", 0.6f);;
		mallet.scale = 0.6f;

		GSpineManager.LoadSpine ("LighteningAtlas", "Atlases/LighteningJson", "Atlases/LighteningAtlas");
		Lightening lightening = new Lightening("LighteningAtlas", 0.6f);
		lightening.scale = 0.6f;
		
		AffectPictureWords tremble = new AffectPictureWords(specialFont, "tremble", glass);
		AffectPictureWords thundercloud = new AffectPictureWords(specialFont, "thundercloud", lightening);
		
		tremble.SetPosition (startX + tremble.textRect.width*2f, Futile.screen.halfWidth);
		thundercloud.SetPosition (startX+thundercloud.textRect.width/1.5f, tremble.y-thundercloud.textRect.height);
		
		Futile.stage.AddChild (tremble);
		specialWords.Add (tremble);
		Futile.stage.AddChild (thundercloud);
		specialWords.Add (thundercloud);
		
		mallet.SetPosition (tremble.x+mallet.height/2f, groundHeight);
		glass.SetPosition (tremble.x, tremble.y);
		lightening.SetPosition (lightening.x, lightening.y);
		mallet.Start();
		glass.Start();
		lightening.Start ();
		
		lightening.addCollider (glass);
		glass.addCollider (mallet);
		
		
	}
	
	void setUpTwinkleStage(float startX)
	{
		FSprite background = new FSprite("blank");
		Futile.stage.AddChild (background);
		background.scale = 0.6f;
		background.SetPosition((background.width/2)-20 + startX, background.height/2);
		
		FSprite background2 = new FSprite("blank");
		Futile.stage.AddChild (background2);
		background2.scale = 0.6f;
		background2.SetPosition((background.width/2)-20 + background.x, background.height/2);
		
		FSprite background3 = new FSprite("blank");
		Futile.stage.AddChild (background3);
		background3.scale = 0.6f;
		background3.SetPosition((background.width/2)-20 + background2.x, background.height/2);
		
		FSprite background4 = new FSprite("blank");
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
		
		mediumText.Add (block1);
		mediumText.Add (block2);
		mediumText.Add (block3);
		
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
		
		AffectPictureWords talk = new AffectPictureWords(specialFont, "talk", twinkleText, 2);
		
		specialWords.Add (talk);
		
		block1.SetPosition (startX+block1.textRect.width, groundHeight + block1.textRect.height*2f);
		block2.SetPosition (block1.x+block2.textRect.width/2f, block1.y + block2.textRect.height*3f);
		talk.SetPosition (block2.x + block2.textRect.width/2.5f, block2.y);
		block3.SetPosition (block2.x, block2.y-block3.textRect.height/2f);
		
		twinkle1.SetPosition (talk.x + twinkle1.textRect.width/2.2f, talk.y+twinkle1.textRect.height*2f);
		twinkle2.SetPosition (twinkle1.x + twinkle2.textRect.width/1.5f, block3.y);
		twinkle3.SetPosition (twinkle2.x + twinkle3.textRect.width/1.5f, twinkle1.y+twinkle3.textRect.height);
		twinkle4.SetPosition (twinkle3.x + twinkle4.textRect.width/1.5f, twinkle3.y+twinkle4.textRect.height*3f);
		
		foreach (MediumText mt in mediumText)
		{
			Futile.stage.AddChild (mt);
			mt.scale = 0.6f;
			Rect mtRect = makeTextRect(mt);
			mediumTextRects.Add (mtRect);
		}
				
		foreach (SpecialWords sw in specialWords)
		{
			Futile.stage.AddChild (sw);
			Rect swRect = makeTextRect(sw);
			specialWordRects.Add (swRect);
		}
		
		foreach (PictureObstacle pic in pictures)
		{
			Rect picRect = pic.localRect.CloneAndOffset(pic.x, pic.y);
			pictureObstacleRects.Add(picRect);
		}
		
	}
	
	Rect makeTextRect(FLabel l)
	{
		Rect r;
		r = new Rect(l.x - 0.6f * l.textRect.width/2, l.y, 0.6f * l.textRect.width, 0.6f * l.textRect.height);
		return r;
	}
	
}