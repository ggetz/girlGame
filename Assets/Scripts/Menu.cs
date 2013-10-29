using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;

// remember to extend FMultiTouchableInterface! (wow that's a long title)
public class Menu: MonoBehaviour
{
	FButton start;
	FButton acheivements;
	FButton quit;
	
	FSprite logo;
	
	void Start()
	{
		Resources.Load("Music/alice");
		
		// Setup Futile
		FutileParams fparams = new FutileParams(true, true, false, false);
		fparams.AddResolutionLevel(1024.0f,	1.0f, 1.0f, "");
		fparams.origin = new Vector2(0f, 0f);
		Futile.instance.Init(fparams);
		
		Futile.atlasManager.LoadAtlas("Atlases/MenuAtlas");
		
		FSprite background = new FSprite("Blog Background");
		//background.scale=0.6f;
		
		background.SetPosition(Futile.screen.width/2f, background.height/2f);
		Futile.stage.AddChild (background);
	
		start = new FButton("start_green", "start_blue");
		acheivements = new FButton("acheive_purple", "acheive_pink");
		quit = new FButton("quit_blue", "quit_purple");
		
		logo = new FSprite("logo");
		
		start.SetPosition (Futile.screen.width*0.4f, Futile.screen.height*0.45f);
		
		acheivements.SetPosition (Futile.screen.width*0.4f, Futile.screen.height*0.2f);
		
		quit.SetPosition(Futile.screen.width*0.4f, Futile.screen.height*0.07f);
		
		logo.SetPosition (Futile.screen.width*0.4f, Futile.screen.height*0.7f);
		
		Futile.stage.AddChild (start);
		Futile.stage.AddChild (quit);
		Futile.stage.AddChild (acheivements);
		Futile.stage.AddChild (logo);
		
		start.SignalRelease+=HandleStartButtonRelease;
		acheivements.SignalRelease+=HandleAcheivementButtonRelease;
		quit.SignalRelease+=HandleQuitButtonRelease;
	}
	
	private void HandleStartButtonRelease(FButton button)
	{
		Application.LoadLevel("Prologue");
	}
	
	private void HandleAcheivementButtonRelease(FButton button)
	{
		Application.LoadLevel("AcheivementScreen");
	}
	
	private void HandleQuitButtonRelease(FButton button)
	{
		Application.Quit();
	}
	
}