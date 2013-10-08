using UnityEngine;
using System.Collections;

public class SpecialWords : MediumText
{
	PictureObstacle effectedObstacle;
	bool linked = false;
	Girl girl;
	public Rectangle rect;
	enum effectTypes{SHRINK, ENLARGE};
	float height;
	float width;
	
	// Use this for initialization
	public SpecialWords(string font, string word, float scale): base(font, word)
	{
		height = textRect.height * scale;
		width = textRect.width * scale;
		x = x - width/2;
		y = y - height/2;
		rect=new Rectangle(x, y, width, height);
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
	
	public bool contactMade()
	{
		return rect.isIntersecting(girl.getRect ());
	}
	

	
}
