using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;


public class Epilogue: MonoBehaviour
{
	
	string textFont;
	GSpineSprite wholeShot;
	GSpineSprite hand;
	GSpineSprite leftArc;
	GSpineSprite rightArc;
	GSpineSprite lowerBody;
	GSpineSprite leg;
	
	FSprite finalHand;
	FSprite finalScene;
	
	GSpineSprite fadeTarget;
	FSprite fadeImage;
	FLabel text;
	int fade;
	float fadeRate;
	bool spineFading;
	bool imageFading;
	bool fadeType;
	int time=0;
	int framerate;
	int factor=1;
	bool pan=false;
	
	bool unplayed=true;
	
	FSprite border1, border2;
	
	void Start()
	{
		// Setup Futile
		FutileParams fparams = new FutileParams(true, true, false, false);
		fparams.AddResolutionLevel(1024.0f,	1.0f, 1.0f, "");
		fparams.origin = new Vector2(0f, 0f);
		Futile.instance.Init(fparams);

		LoadTextures ();
		SetUpStage ();
		
	}
	
	void LoadTextures()
	{
		// Load the girl 
		GSpineManager.LoadSpine("EpilogueWholeShotAtlas", "Atlases/EpilogueWholeShotJson", "Atlases/EpilogueWholeShotAtlas");
		wholeShot = new GSpineSprite("EpilogueWholeShotAtlas");
		
		GSpineManager.LoadSpine("HandFocusAtlas", "Atlases/HandFocusJson", "Atlases/HandFocusAtlas");
		hand = new GSpineSprite("HandFocusAtlas");
		
		GSpineManager.LoadSpine("LeftArcAtlas", "Atlases/LeftArcJson", "Atlases/LeftArcAtlas");
		leftArc = new GSpineSprite("LeftArcAtlas");
		
		GSpineManager.LoadSpine("RightArcAtlas", "Atlases/RightArcJson", "Atlases/RightArcAtlas");
		rightArc = new GSpineSprite("RightArcAtlas");
		
		GSpineManager.LoadSpine("LowerBodyAtlas", "Atlases/LowerBodyJson", "Atlases/LowerBodyAtlas");
		lowerBody = new GSpineSprite("LowerBodyAtlas");
		
		GSpineManager.LoadSpine("FeetCloseUpAtlas", "Atlases/FeetCloseUpJson", "Atlases/FeetCloseUpAtlas");
		leg = new GSpineSprite("FeetCloseUpAtlas");
		
		Futile.atlasManager.LoadAtlas ("Atlases/EpilogueAtlas");
		finalHand=new FSprite("FinalHand");
		finalScene=new FSprite("FinalScene");	
		border1=new FSprite("BlackEdge");
		border2=new FSprite("BlackEdge");

	}
			
	void SetUpStage()
	{
		Futile.stage.AddChild (wholeShot);
		Futile.stage.AddChild (leg);
		Futile.stage.AddChild (rightArc);
		Futile.stage.AddChild (leftArc);
		Futile.stage.AddChild(lowerBody);
		Futile.stage.AddChild (hand);
		Futile.stage.AddChild(finalScene);
		Futile.stage.AddChild(finalHand);
		
		wholeShot.SetPosition(Futile.screen.width/2f, 0);
		leg.SetPosition(Futile.screen.width/2f, 0);
		hand.SetPosition(Futile.screen.width/2f, 0);
		rightArc.SetPosition(Futile.screen.width/2f, 0);
		leftArc.SetPosition(Futile.screen.width/2f, 0);
		lowerBody.SetPosition(Futile.screen.width/2f, 0);
		finalHand.SetPosition(Futile.screen.width/2f, Futile.screen.height/2f);
		finalScene.SetPosition(Futile.screen.width/1.5f, Futile.screen.height/7f);
		
		wholeShot.alpha = 0f;
		leg.alpha=0f;
		rightArc.alpha=0f;
		hand.alpha=0f;
		leftArc.alpha=0f;
		lowerBody.alpha=0f;
		finalScene.alpha=0f;
		finalHand.alpha=0f;
		
		border1.scale=0.6f;
		border2.scale=0.6f;
		
		border1.SetPosition(border1.width/2f, border1.height/2f);
		border2.SetPosition(Futile.screen.width-border2.width/2f, border2.height/2f);
		
		Futile.stage.AddChild(border1);
		Futile.stage.AddChild (border2);
	}
	
	void Update()
	{	
		if(time==0)
		{
			fadeScene (true, 50, wholeShot);
			wholeShot.Play("Part1", false);
		}
		
		
		if(time>=100 && time<110 && unplayed)
		{
			Debug.Log ("2");
			wholeShot.Stop ();
			wholeShot.alpha=0;
			leg.alpha=100;
			leg.Play ("Part1", false);
			unplayed=false;
		}
		
		if(time>=110 && time<=120 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=130 && time<=140 && unplayed)
		{
			leg.Stop ();
			leg.alpha=0;
			wholeShot.alpha=100;
			wholeShot.Play ("Part2", false);
			unplayed=false;
		}
		
		if(time>=300 && time<=310 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=360 && time<=370 && unplayed)
		{
			wholeShot.Stop ();
			wholeShot.alpha=0;
			leg.alpha=100;
			leg.Play("Part2", false);
			unplayed=false;
		}
		
		if(time>=500 && time<=510 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=550 && time<=560 && unplayed)
		{
			leg.Stop ();
			leg.alpha=0;
			wholeShot.alpha=100;
			wholeShot.Play ("Part3", false);
			unplayed=false;
		}
		
		if(time>=800 && time<=810 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1280 && time<=1290 && unplayed)
		{
			wholeShot.Stop ();
			wholeShot.alpha=0;
			hand.alpha=100;
			hand.Play ("Part1", false);
			unplayed=false;
		}
		
		if(time>=1500 && time<=1510 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1620 && time<=1630 && unplayed)
		{
			hand.alpha=0;
			hand.Stop ();
			wholeShot.alpha=100;
			wholeShot.Play ("Part4", false);
			unplayed=false;
		}
		
		if(time>=1700 && time<=1710 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1800 && time<=1810 && unplayed)
		{
			wholeShot.alpha=0;
			wholeShot.Stop();
			rightArc.alpha=100;
			rightArc.Play("Part1", false);
			unplayed=false;
		}
		
		if(time>=1850 && time<=1860 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1880 && time<=1890 && unplayed)
		{
			rightArc.alpha=0;
			rightArc.Stop();
			leftArc.alpha=100;
			leftArc.Play ("Part1", false);
			unplayed=false;
		}
		
		if(time>=1900 && time<=1910 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1950 && time<=1960 && unplayed)
		{
			fadeScene(false, 50, leftArc);
			unplayed=false;
		}
		
		if(time>=2000 && time<=2010 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=2100 && time<=2110 && unplayed)
		{
			fadeScene (true, 50, finalHand);
			leftArc.Stop ();
			unplayed=false;
		}
		
		if(time>=2200 && time<=2210 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=2300 && time<=2310 && unplayed)
		{
			fadeScene (false, 50, finalHand);
			unplayed=false;
		}
		
		if(time>=2400 && time<=2410 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=2450 && time<=2460 && unplayed)
		{
			fadeScene (true, 50, finalScene);
			pan=true;
			unplayed=false;
		}
	
		if(time>=2700 && time<=2710 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=2800 && time<=2810 && unplayed)
		{
			fadeScene (false, 50, finalScene);
			unplayed=false;
		}
		
		if(time>=2900 && time<=2910 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=3000 && time<=3010 && unplayed)
		{
			finalScene.scale=0.6f;
			finalScene.SetPosition (Futile.screen.width/2f, Futile.screen.height/2f);
			pan=false;
			fadeScene (true, 50, finalScene);
			unplayed=false;
		}
		
		if(time>=3100 && time<=3110 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=3300 && time<=3310 && unplayed)
		{
			unplayed=true;
			fadeScene (false, 50, finalScene);
		}
		
		if(time>=3500)
		{
			Application.LoadLevel ("MainMenu");
		}
		
		framerate=(int)(1.0f / Time.deltaTime);
		factor=60/framerate;
		
		if(factor==0)
		{
			factor=1;
		}
		else if(factor>=10)
		{
			factor=8;
		}
		
		time+=(int)(factor+0.5);
		if(spineFading)
		{
			if(fade<0)
			{
				fadeScene (fadeType, fade, fadeTarget);
			}
			if(fadeType)
			{
				fadeTarget.alpha+=fadeRate;
			}
			else
			{
				fadeTarget.alpha-=fadeRate;
			}
			fade--;
		}
		
		if(imageFading)
		{
			if(fade<0)
			{
				fadeScene (fadeType, fade, fadeImage);
			}
			if(fadeType)
			{
				fadeImage.alpha+=fadeRate;
			}
			else
			{
				fadeImage.alpha-=fadeRate;
			}
			fade--;	
		}
		
		if(pan)
		{
			panRight (finalScene);
		}
	}
	
	void fadeScene(bool fadeIn, int fadeTime, GSpineSprite target)
	{
		if(fadeTime > 0 && !spineFading)
		{
			spineFading=true;
			fadeRate=1f/fadeTime;
			fade=fadeTime;
			fadeType=fadeIn;
			fadeTarget=target;
		}
		
		if(fadeTime <=0 && spineFading)
		{
			spineFading=false;
			fadeRate=0;
			fadeTime=0;
			fadeType=false;
			fadeTarget=null;
		}
		
	}
	
	void fadeScene(bool fadeIn, int fadeTime, FSprite target)
	{
		if(fadeTime > 0 && !imageFading)
		{
			imageFading=true;
			fadeRate=1f/fadeTime;
			fade=fadeTime;
			fadeType=fadeIn;
			fadeImage=target;
		}
		
		if(fadeTime <=0 && imageFading)
		{
			imageFading=false;
			fadeRate=0;
			fadeTime=0;
			fadeType=false;
			fadeImage=null;
		}
		
	}
	
	void panRight(FSprite image)
	{
		image.x--;
	}
	
}