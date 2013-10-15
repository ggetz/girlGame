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
		
		Futile.atlasManager.LoadFont("PalatinoMedium", "MediumNormalText", "Atlases/MediumNormalText", 0, 0);
		//Futile.atlasManager.LoadFont("PalatinoSpecial", "MediumSpecialText", "Atlases/MediumSpecialText", 0, 0);
		
		Futile.atlasManager.LoadAtlas("Atlases/MenuAtlas");
		
		FSprite background = new FSprite("Blog Background");
		background.scale=0.6f;
		
		background.SetPosition(Futile.screen.width/2f, background.height/2f);
		Futile.stage.AddChild (background);
	
		back = new FButton("StartButton1", "StartButton2");
		
		back.scale=0.2f;
		back.SetPosition (Futile.screen.width*0.05f, Futile.screen.height*0.05f);
		
		Futile.stage.AddChild (back);
		
		back.SignalRelease+=HandleBackButtonRelease;
		
		acheives = new List<Acheivement>();
		acheives.Add(new Acheivement("Fast!", "Complete Alice in Wonderland in less than a minute."));
		acheives.Add(new Acheivement("Doodles", "Collect all doodles."));
		acheives.Add(new Acheivement("Acheivement", "And this is the description."));
		
		float y = Futile.screen.height * 0.8f;
		
		foreach (Acheivement a in acheives)
		{
			FLabel title = new FLabel("PalitinoMedium", a.title);
			title.SetPosition(Futile.screen.width * 0.1f, y );
			if (!a.isComplete())
				title.alpha = 0.5f;
			Futile.stage.AddChild(title);
			
			FLabel desc = new FLabel("PalitinoMedium", a.desc);
			desc.SetPosition(Futile.screen.width * 0.1f, y + Futile.screen.height * 0.1f );
			if (!a.isComplete())
				desc.alpha = 0.5f;
			Futile.stage.AddChild(desc);
				
			y += Futile.screen.height * 0.2f;
		}
	}
	
	private void HandleBackButtonRelease(FButton button)
	{
		Application.LoadLevel("MainMenu");
	}
	
}