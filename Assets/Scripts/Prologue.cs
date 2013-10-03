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
		
		if(time==250)
		{
			text.text="They can't hear us but we cry\nas if they could";
			fadeText (true, 50);
		}
		
		if(time==450)
		{
			fadeText (false, 50);
		}
		if(time==700)
		{
			wholeShot.alpha=0;
			mouth.alpha=100;
			mouth.Play("Part1", false);
		}
		
		if(time==950)
		{
			text.text="Even if only for an instant\nlonger,even if we will fade\none day...";
			fadeText (true, 50);
		}
		if(time==1150)
		{
			fadeText (false, 50);
		}
		
		if(time==1200)
		{
			mouth.alpha=0;
			wholeShot.Stop ();
			wholeShot.alpha=100;
			wholeShot.Play("Part2", false);
		}
		
		if(time==1350)
		{
			wholeShot.alpha=0;
			eye.alpha=100;
			eye.Play("animation", false);
		}
		
		if(time==1500)
		{
			text.text="Please.\n\nNot Yet";
			fadeText (true, 50);
		}
		
		if(time==1700)
		{
			fadeText (false,50);
		}
		
		if(time==1800)
		{
			eye.alpha=0;
			eye.Stop ();
			wholeShot.Stop ();
			wholeShot.alpha=100;
			
			wholeShot.Play("Part3", false);
		}
		
		if(time==2000)
		{
			wholeShot.Stop();
			wholeShot.alpha=0;
			hand.alpha=100;
			hand.Play("Part1", false);
		}
		
		if(time==2400)
		{
			hand.alpha=0;
			wholeShot.alpha=100;
			wholeShot.Play("Part4", false);
		}
		
		if(time==3000)
		{
			text.text="So you over there, the\none who can hear us.";
			fadeText (true, 50);
		}
		
		if(time==3150)
		{
			fadeText (false, 50);
		}
		
		if(time==3000)
		{
			wholeShot.alpha=0;
			hand.alpha=100;
			hand.Play("Part2", false);
		}
		
		if(time==3500)
		{
			text.text="Won't you carry us far\naway from here?";
			fadeText (true, 50);
		}
		if(time==3800)
		{
			fadeText (false, 50);
		}
		
		time++;
		
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