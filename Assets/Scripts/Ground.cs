using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Ground
{
	float groundHeight;
	string gFont;
	
	public Ground(float ground, string font)
	{
		groundHeight = ground;
		gFont = font;
		
	}

	// Use this for initialization
	public void Start () 
	{
		try
        {
            using (StreamReader sr = new StreamReader("Assets/Resources/AliceGround.txt"))
            {
                string text = sr.ReadToEnd ();
                MediumText groundText = new MediumText(gFont, text);
				groundText.scale = 0.6f;
				groundText.SetPosition (groundText.textRect.width/3.334f, groundHeight);
				Futile.stage.AddChild (groundText);
            }
        }
        catch (Exception e)
        {
            Debug.Log ("ooops");
			Debug.Log (e);
        }
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

}
