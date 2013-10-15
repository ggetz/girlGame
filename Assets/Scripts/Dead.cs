using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// remember to extend FMultiTouchableInterface! (wow that's a long title)
public class Dead: MonoBehaviour
{
	FButton replay;
	float rotation = 0f;
	FSprite girl;
	void Start()
	{
		// Setup Futile
		FutileParams fparams = new FutileParams(true, true, false, false);
		fparams.AddResolutionLevel(1024.0f,	1.0f, 1.0f, "");
		fparams.origin = new Vector2(0f, 0f);
		Futile.instance.Init(fparams);
		
		Futile.atlasManager.LoadAtlas("Atlases/DeadScreenAtlas");
		
		girl = new FSprite("DeathScreenIcon");
		girl.scale=0.6f;
		
		girl.SetPosition(Futile.screen.width/2f, Futile.screen.height/1.5f);
		Futile.stage.AddChild (girl);
	
		replay = new FButton("ReplayButton", "ReplayButton2");
		replay.scale=0.6f;
		replay.SetPosition (Futile.screen.width/2f, Futile.screen.height/4f);
		Futile.stage.AddChild (replay);
		
		replay.SignalRelease+=HandleStartButtonRelease;
	}
	
	void Update()
	{
		rotation+=0.2f;
		girl.rotation=rotation;
	}
	
	private void HandleStartButtonRelease(FButton button)
	{
		Application.LoadLevel("MainMenu");
	}
}