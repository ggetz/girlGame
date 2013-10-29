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
	
	public void Start () 
	{
		rect.x=x;
		rect.y=y-height/2f;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void newRect(Rectangle r)
	{
		rect=r;
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
	
	/**public Rectangle getRect()
	{
<<<<<<< HEAD
		return rect;
	}
=======
		return
	}**/
>>>>>>> a6b6a4805f899273890c3ac9bfe277291bf0ef10
}
