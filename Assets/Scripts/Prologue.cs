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
	}
			
	void SetUpStage()
	{
		
		Futile.stage.AddChild (wholeShot);
		Futile.stage.AddChild (eye);
		Futile.stage.AddChild (mouth);
		Futile.stage.AddChild (hand);
		Futile.stage.AddChild(text);
		
		wholeShot.SetPosition(1920*0.6f/2f, 0);
		eye.SetPosition(1920*0.6f/2f, 0);
		hand.SetPosition(1920*0.6f/2f, 0);
		mouth.SetPosition(1920*0.6f/2f, 0);
		
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
		
		if(time==700)
		{
			wholeShot.alpha=0;
			mouth.alpha=100;
			mouth.Play("Part1", false);
		}
		
		if(time==1100)
		{
			mouth.alpha=0;
			wholeShot.alpha=100;
			wholeShot.Play("Part2", false);
		}
		
		if(time==1250)
		{
			wholeShot.alpha=0;
			eye.alpha=100;
			eye.Play("animation", false);
		}
		
		if(time==1700)
		{
			eye.alpha=0;
			wholeShot.alpha=100;
			eye.Stop();
			wholeShot.Play("Part3", false);
		}
		
		if(time==1900)
		{
			wholeShot.Stop();
			wholeShot.alpha=0;
			hand.alpha=100;
			hand.Play("Part1", false);
		}
		
		if(time==2300)
		{
			hand.alpha=0;
			wholeShot.alpha=100;
			wholeShot.Play("Part4", false);
		}
		
		if(time==2950)
		{
			wholeShot.alpha=0;
			hand.alpha=100;
			hand.Play("Part2", false);
		}
		
		time++;
	}
	
	void textFade(bool fadeIn, int startTime)
	{
			
	}
	
}