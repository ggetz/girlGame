using UnityEngine;
using System.Collections;

public class SpecialWords : MediumText
{
	PictureObstacle effectedObstacle;
	enum effectTypes{SHRINK, ENLARGE};
	float height;
	float width;
	bool isActivated;
	Rectangle rect;
	
	// Use this for initialization
	public SpecialWords(string font, string word, float scale): base(font, word)
	{
		height = textRect.height * scale;
		width = textRect.width * scale;
		x = x - width/2;
		y = y - height/2;
		alpha=alpha*0.5f;
	}
	
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public virtual void action()
	{
	}
	
	public Rectangle getRect()
	{
		return rect;
	}
	
	public void setRect(Rectangle r)
	{
		rect=r;
	}
	
	public void activate()
	{
		alpha=alpha*2f;
		isActivated=true;
	}
	
	public void deactivate()
	{
		alpha=alpha*0.5f;
		isActivated=false;
	}
	
	public bool getActive()
	{
		return isActivated;
	}
	
}
