using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;

// remember to extend FMultiTouchableInterface! (wow that's a long title)
public class AcheivementMenu: MonoBehaviour
{
	FButton back;
	List<Acheivement> acheives;
		
	void Start()
	{
		// Setup Futile
		FutileParams fparams = new FutileParams(true, true, false, false);
		fparams.AddResolutionLevel(1024.0f,	1.0f, 1.0f, "");
		fparams.origin = new Vector2(0f, 0f);
		Futile.instance.Init(fparams);
		
		Futile.atlasManager.LoadAtlas("Atlases/MenuAtlas");
		Futile.atlasManager.LoadFont("MediumNormalText", "MediumNormalText", "Atlases/MediumNormalText", 0, 0);
		
		FSprite background = new FSprite("Blog Background");
		//background.scale=0.6f;
		
		background.SetPosition(Futile.screen.width/2f, background.height/2f);
		Futile.stage.AddChild (background);
	
		back = new FButton("back_pink", "back_purple");
		
		back.scale=0.6f;
		back.SetPosition (Futile.screen.width*0.1f, Futile.screen.height*0.1f);
		
		Futile.stage.AddChild (back);
		
		back.SignalRelease+=HandleBackButtonRelease;
		
		acheives = new List<Acheivement>();
		acheives.Add(new Acheivement("Down the Rabbit Hole", "Complete Alice in Wonderland."));
		acheives.Add(new Acheivement("Caucus Race Champion", "Complete Alice in Wonderland in less than a minute."));
		acheives.Add(new Acheivement("Savior of Wonderland", "Collect all doodles in Alice in Wonderland."));
		acheives[2].Complete();
		acheives.Add(new Acheivement("Queen's Immunity", "Complete Alice in Wonderland without being erased once."));
		
		float y = Futile.screen.height * 0.95f;
		
		foreach (Acheivement a in acheives)
		{
			FLabel title = new FLabel("MediumNormalText", a.title);
			title.SetPosition(Futile.screen.width * 0.5f, y );
			title.scale = 0.8f;
			if (!a.isComplete())
				title.alpha = 0.2f;
			Futile.stage.AddChild(title);
			
			FLabel desc = new FLabel("MediumNormalText", a.desc);
			desc.SetPosition(Futile.screen.width * 0.5f, y - Futile.screen.height * 0.05f );
			desc.scale = 0.5f;
			if (!a.isComplete())
				desc.alpha = 0.2f;
			Futile.stage.AddChild(desc);
				
			y -= Futile.screen.height * 0.15f;
		}
	}
	
	private void HandleBackButtonRelease(FButton button)
	{
		Application.LoadLevel("MainMenu");
	}
	
}
