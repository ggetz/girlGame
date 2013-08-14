using UnityEngine;
using System.Collections;

public class MovingPictureObstacles : GSpineSprite 
{

	private bool solid=true;
	
	// Use this for initialization
	public MovingPictureObstacles(string atlas) : base(atlas)
	{
		
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
	
	public virtual void releaseAction()
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
}
