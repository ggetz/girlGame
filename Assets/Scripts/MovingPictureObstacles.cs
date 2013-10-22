using UnityEngine;
using System.Collections;

public class MovingPictureObstacles : GSpineSprite 
{

	private bool solid=true;
	private float width;
	private float height;
	private float oscale;
	Rectangle rect;
	// Use this for initialization
	public MovingPictureObstacles(string atlas) : base(atlas)
	{
		
	}
	
	public virtual void Start () 
	{
		
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
	
	}
	
	public virtual void action()
	{
		
	}
	
	public virtual void electrify()
	{
	}
	
	public void setSolidity(bool solidity)
	{
		solid = solidity;
	}
	
	public virtual Rectangle getRect()
	{
		return rect;
	}
	
	public bool isSolid()
	{
		return solid;
	}
}
