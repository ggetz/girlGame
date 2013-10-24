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
	float scaling=0.5f;
	int framerate;
	int factor=1;
	bool pan=false;
	
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

		leftArc.scale=scaling;
		rightArc.scale=scaling;
		lowerBody.scale=scaling;
		leg.scale=scaling;
		hand.scale=scaling;
		finalHand.scale=scaling;
		wholeShot.scale=scaling;
		

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
		
		
	}
	
	void Update()
	{	
		if(time==0)
		{
			fadeScene (true, 50, wholeShot);
			wholeShot.Play("Part1", false);
		}
		
		if(time==100)
		{
			wholeShot.Stop ();
			wholeShot.alpha=0;
			leg.alpha=100;
			leg.Play ("Part1", false);
		}
		
		if(time==130)
		{
			leg.Stop ();
			leg.alpha=0;
			wholeShot.alpha=100;
			wholeShot.Play ("Part2", false);
		}
		if(time==360)
		{
			wholeShot.Stop ();
			wholeShot.alpha=0;
			leg.alpha=100;
			leg.Play("Part2", false);
		}
		
		if(time==550)
		{
			leg.Stop ();
			leg.alpha=0;
			wholeShot.alpha=100;
			wholeShot.Play ("Part3", false);
		}
		
		if(time==1280)
		{
			wholeShot.Stop ();
			wholeShot.alpha=0;
			hand.alpha=100;
			hand.Play ("Part1", false);
		}
		
		if(time==1620)
		{
			hand.alpha=0;
			hand.Stop ();
			wholeShot.alpha=100;
			wholeShot.Play ("Part4", false);
		}
		
		
		if(time==1800)
		{
			wholeShot.alpha=0;
			wholeShot.Stop();
			rightArc.alpha=100;
			rightArc.Play("Part1", false);
		}
		
		if(time==1880)
		{
			rightArc.alpha=0;
			rightArc.Stop();
			leftArc.alpha=100;
			leftArc.Play ("Part1", false);
		}
		
		if(time==1950)
		{
			fadeScene(false, 50, leftArc);
		}
		
		if(time==2100)
		{
			fadeScene (true, 50, finalHand);
			leftArc.Stop ();
		}
		
		if(time==2300)
		{
			fadeScene (false, 50, finalHand);
		}
		
		if(time==2450)
		{
			fadeScene (true, 50, finalScene);
			pan=true;
		}
	
		if(time==2800)
		{
			fadeScene (false, 50, finalScene);
		}
		
		if(time==3000)
		{
			finalScene.scale=scaling;
			finalScene.SetPosition (Futile.screen.width/2f, Futile.screen.height/2f);
			pan=false;
			fadeScene (true, 50, finalScene);
		}
		
		if(time==3300)
		{
			fadeScene (false, 50, finalScene);
		}
		
		if(time==3500)
		{
			Application.LoadLevel ("MainMenu");
		}
		
		time++;
		
				
		if(time>15)
		{
			framerate=(int)(1.0f / Time.deltaTime);
			Debug.Log ("Framerate: " + framerate);
			factor=60/framerate;
			
			if(factor==0)
			{
				factor=1;
			}
		}
		
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