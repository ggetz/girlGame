using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;


public class Prologue: MonoBehaviour
{
	
	string textFont;
	GSpineSprite wholeShot;
	GSpineSprite hand;
	GSpineSprite eye;
	GSpineSprite mouth;
	FLabel text;
	int fade;
	float fadeRate;
	bool textFading;
	bool fadeType;
	int time=0;
	int framerate;
	int factor=1;
	
	bool unplayed=true;
	
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
		GSpineManager.LoadSpine("PrologueWholeShotAtlas", "Atlases/PrologueWholeShotJson", "Atlases/PrologueWholeShotAtlas");
		wholeShot = new GSpineSprite("PrologueWholeShotAtlas");
		wholeShot.scale=0.6f;
		
		GSpineManager.LoadSpine("HandCloseUpAtlas", "Atlases/HandCloseUpJson", "Atlases/HandCloseUpAtlas");
		hand = new GSpineSprite("HandCloseUpAtlas");
		
		GSpineManager.LoadSpine("EyeCloseUpAtlas", "Atlases/EyeCloseUpJson", "Atlases/EyeCloseUpAtlas");
		eye = new GSpineSprite("EyeCloseUpAtlas");
		
		GSpineManager.LoadSpine("MouthCloseUpAtlas", "Atlases/MouthCloseUpJson", "Atlases/MouthCloseUpAtlas");
		mouth = new GSpineSprite("MouthCloseUpAtlas");
		
		
		eye.scale=0.6f;
		mouth.scale=0.6f;
		hand.scale=0.6f;
		
		//load the atlas
		Futile.atlasManager.LoadAtlas("Atlases/PrologueAtlas");
		Futile.atlasManager.LoadFont("Incubus", "Incubus", "Atlases/Incubus", 0, 0);
		textFont = "Incubus";
		
		text = new FLabel(textFont, "");
		text.scale=0.6f;
	}
			
	void SetUpStage()
	{
		
		Futile.stage.AddChild (wholeShot);
		Futile.stage.AddChild (eye);
		Futile.stage.AddChild (mouth);
		Futile.stage.AddChild (hand);
		Futile.stage.AddChild(text);
		
		wholeShot.SetPosition(Futile.screen.width/2f, 0);
		eye.SetPosition(Futile.screen.width/2f, 0);
		hand.SetPosition(Futile.screen.width/2f, 0);
		mouth.SetPosition(Futile.screen.width/2f, 0);
		text.SetPosition (Futile.screen.width/2f, Futile.screen.height/2f);
		
		wholeShot.alpha = 0f;
		eye.alpha=0f;
		mouth.alpha=0f;
		hand.alpha=0f;
		text.alpha=0f;
		
		
		
	}
	
	void Update()
	{
		
		if(time==0)
		{
			wholeShot.alpha=100;
			wholeShot.Play("Part1", false);
		}
		
		if(time>=250 && time<260 && unplayed)
		{
			text.text="They can't hear us but we cry\nas if they could";
			fadeText (true, 50);
			unplayed=false;
		}
		
		if(time>440 && time<450 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=450 && time<460 && unplayed)
		{
			fadeText (false, 50);
			unplayed=false;
		}
		
		if(time>=680 && time<690 && !unplayed)
		{
			unplayed=true;
		}
		if(time>=700 && time<710 && unplayed)
		{
			wholeShot.alpha=0;
			mouth.alpha=100;
			mouth.Play("Part1", false);
			unplayed=false;
		}
		
		if(time>940 && time<950 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=950 && time<960 && unplayed)
		{
			text.text="Even if only for an instant\nlonger,even if we will fade\none day...";
			fadeText (true, 50);
			unplayed=false;
		}
		
		if(time>=1100 && time<1110 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1150 && time<1160 && unplayed)
		{
			fadeText (false, 50);
			unplayed=false;
		}
		
		if(time>=1170 && time<1180 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time==1200)
		{
			mouth.alpha=0;
			wholeShot.Stop ();
			wholeShot.alpha=100;
			wholeShot.Play("Part2", false);
			unplayed=false;
		}
		
		if(time>=1330 && time<1340 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1350 && time<1360 && unplayed)
		{
			wholeShot.alpha=0;
			eye.alpha=100;
			eye.Play("animation", false);
			unplayed=false;
		}
		
		if(time>=1450 && time<1460 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1500 && time<1510 && unplayed)
		{
			text.text="Please.\n\nNot Yet";
			fadeText (true, 50);
			unplayed=false;
		}
		
		if(time>=1650 && time<1660 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1700 && time<1710 && unplayed)
		{
			fadeText (false,50);
			unplayed=false;
		}
		
		if(time>=1780 && time<1790 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=1800 && time<1810 && unplayed)
		{
			eye.alpha=0;
			eye.Stop ();
			wholeShot.Stop ();
			wholeShot.alpha=100;
			
			wholeShot.Play("Part3", false);
			unplayed=false;
		}
		
		if(time>=1950 && time<1960 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=2000 && time<2010 && unplayed)
		{
			wholeShot.Stop();
			wholeShot.alpha=0;
			hand.alpha=100;
			hand.Play("Part1", false);
			unplayed=false;
		}
		
		if(time>=2350 && time<2360 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=2400 && time<2410 && unplayed)
		{
			hand.alpha=0;
			wholeShot.alpha=100;
			wholeShot.Play("Part4", false);
			unplayed=false;
		}
		
		if(time>=2900 && time<2910 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=3000 && time<3010 && unplayed)
		{
			text.text="So you over there, the\none who can hear us.";
			fadeText (true, 50);
			unplayed=false;
		}
		
		if(time>=3100 && time<3110 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=3150 && time<3160 && unplayed)
		{
			fadeText (false, 50);
			unplayed=false;
		}
		
		if(time>=3020 && time<3030 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=3050 && time<3060 && unplayed)
		{
			wholeShot.alpha=0;
			hand.alpha=100;
			hand.Play("Part2", false);
			unplayed=false;
		}
		
		if(time>=3400 && time<3410 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=3500 && time<3510 && unplayed)
		{
			text.text="Won't you carry us far\naway from here?";
			fadeText (true, 50);
			unplayed=false;
		}
		
		if(time>=3600 && time<3610 && !unplayed)
		{
			unplayed=true;
		}
		
		if(time>=3650 && time<3660 && unplayed)
		{
			fadeText (false, 50);
			unplayed=false;
		}
		
		if(time>=3710)
		{
			Application.LoadLevel ("MainMenu");
		}

		framerate=(int)(1.0f / Time.deltaTime);
		Debug.Log ("Framerate: " + framerate);
		factor=60/framerate;
		
		if(factor==0)
		{
			factor=1;
		}
		
		time+=(int)factor;
		
		
		if(textFading)
		{
			if(fade<0)
			{
				fadeText (fadeType, fade);
			}
			if(fadeType)
			{
				text.alpha+=fadeRate;
			}
			else
			{
				text.alpha-=fadeRate;
			}
			fade--;
		}
	}
	
	void fadeText(bool fadeIn, int fadeTime)
	{
		Debug.Log (fadeTime);
		Debug.Log (fadeIn);
		if(fadeTime > 0 && !textFading)
		{
			textFading=true;
			fadeRate=1f/fadeTime;
			fade=fadeTime;
			fadeType=fadeIn;
		}
		
		if(fadeTime <=0 && textFading)
		{
			textFading=false;
			fadeRate=0;
			fadeTime=0;
			fadeType=false;
		}
	}
	
}