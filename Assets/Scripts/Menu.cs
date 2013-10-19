﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;

// remember to extend FMultiTouchableInterface! (wow that's a long title)
public class Menu: MonoBehaviour
{
	FButton start;
	FButton acheivements;
	
	void Start()
	{
		// Setup Futile
		FutileParams fparams = new FutileParams(true, true, false, false);
		fparams.AddResolutionLevel(1024.0f,	1.0f, 1.0f, "");
		fparams.origin = new Vector2(0f, 0f);
		Futile.instance.Init(fparams);
		
		Futile.atlasManager.LoadAtlas("Atlases/MenuAtlas");
		
		FSprite background = new FSprite("Blog Background");
		background.scale=0.6f;
		
		background.SetPosition(Futile.screen.width/2f, background.height/2f);
		Futile.stage.AddChild (background);
	
		start = new FButton("ReplayButton", "ReplayButton2");
		acheivements = new FButton("ReplayButton", "ReplayButton2");
		
		start.scale=0.6f;
		start.SetPosition (Futile.screen.width*0.4f, Futile.screen.height*0.3f);
		
		acheivements.scale = 0.5f;
		acheivements.SetPosition (Futile.screen.width*0.4f, Futile.screen.height*0.15f);
		
		Futile.stage.AddChild (start);
		Futile.stage.AddChild (acheivements);
		
		start.SignalRelease+=HandleStartButtonRelease;
		acheivements.SignalRelease+=HandleAcheivementButtonRelease;
	}
	
	private void HandleStartButtonRelease(FButton button)
	{
		Application.LoadLevel("AliceLevel");
	}
	
	private void HandleAcheivementButtonRelease(FButton button)
	{
		Application.LoadLevel("AcheivementScreen");
	}
	
}