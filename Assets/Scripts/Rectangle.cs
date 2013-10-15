using UnityEngine;
using System.Collections;

public class Rectangle {

	public float x, y;
	public float height, width;
	public bool isSolid;

	
	public Rectangle(float x, float y, float width = 0, float height = 0)
	{
		this.x = x;
		this.y = y;
		this.height = height;
		this.width = width;
		isSolid = true;
	}
	
	public Rectangle( MediumText text, float scale )
	{
		height = text.textRect.height * scale;
		width = text.textRect.width * scale;
		x = text.x - width/2;
		y = text.y - height/2;
		isSolid = text.isSolid();
	}
	
	//because rects and text rect decide be indiffernt coordinate systems,
	//we need two different "contructors" that take Rects as parameters,
	//and make a rectangle in the proper way	
	public static Rectangle rectangleFromRect(Rect r)
	{
		return new Rectangle (r.x, r.y, r.width, r.height);
	}
	
	public static Rectangle rectangleFromTextRect(Rect r)
	{
		return new Rectangle (r.x, r.y, r.width, r.height);
	}
	
	public float top()
	{
		return y + height;
	}
	
	public float bottom()
	{
		return y;
	}
	
	public float left()
	{
		return x;
	}
	
	public float right()
	{
		return x + width;
	}
	
	public Vector2 center()
	{
		return new Vector2(x + width/2, y + height/2);	
	}
	
	public bool isIntersecting(Rectangle r)
	{
		
		if ( ( ( right() >= r.left() && right() <= r.right() ) || (left() >= r.left () && left() <= r.right())) && (( top () <= r.top() && top() >= r.bottom() )  || ( bottom () <= r.top() && bottom() >= r.bottom() ) ) )
			return true;
		else return false;
	}
	
	public bool doesContain(Vector2 v)
	{
		return (v.x > left() && v.x < right() && v.y > top() && v.y < bottom());
		
	}
	
	public Vector2[] corners()
	{
		Vector2[] corners = new Vector2[4];
		corners[0] = new Vector2(left(), top());
		corners[1] = new Vector2(right(), top());
		corners[2] = new Vector2(right(), bottom());
		corners[3] = new Vector2(left(), bottom());
		return corners;
	}
	
	public void scale(float sc)
	{
		height=height*sc;
		width=width*sc;
	}
}
