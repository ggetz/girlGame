using UnityEngine;
using System.Collections;

public class Ground
{
	float nextX;
	float xLoc=0;
	float yLoc, groundHeight;
	FSprite block;
	string sprite;
	public Ground(float ground)
	{
		groundHeight = ground;
		
	}

	// Use this for initialization
	public void Start () 
	{
		for(int i=0; i < 2; i++)
		{
			for(int x=0; x<=i; x++)
			{
				sprite+="0";
			}
			
			block = new FSprite(sprite);
			
			Futile.stage.AddChild (block);
			block.SetPosition(xLoc+block.width/2, groundHeight - block.height/2.3f);
			xLoc += block.width;
			sprite="";
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public float getXLoc()
	{
		return xLoc;
	}
}
