using UnityEngine;
using System.Collections;

public class PictureObstacle : FSprite 
{

	private bool solid=true;
	Rectangle rect;
	
	// Use this for initialization
	public PictureObstacle(string picture) : base(picture)
	{
		rect=new Rectangle(x, y-(height/2f)*scale, width*scale, height*scale);
	}
	
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public virtual void action()
	{
		
	}
	
	public void setSolidity(bool solidity)
	{
		solid = solidity;
	}
	
	public bool isSolid()
	{
		return solid;
	}
	
	public Rectangle getRect()
	{
		
	}
}
