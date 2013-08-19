using UnityEngine;
using System.Collections;

public class MovingPictureObstacles : GSpineSprite 
{

	private bool solid=true;
	private float width;
	private float height;
	private float oscale;
	
	private Rect rect;
	// Use this for initialization
	public MovingPictureObstacles(string atlas) : base(atlas)
	{
		
	}
	
	void Start () 
	{
		rect = new Rect(x,y,1,1);
	}
	
	// Update is called once per frame
	void Update () 
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
	
	public virtual Rect getRect()
	{
		return rect;
	}
	
	public bool isSolid()
	{
		return solid;
	}
}
