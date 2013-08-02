using UnityEngine;
using System.Collections;

public class PictureObstacle : FSprite 
{

	private bool solid=true;
	
	// Use this for initialization
	public PictureObstacle(string picture) : base(picture)
	{
		
	}
	
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void action()
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
