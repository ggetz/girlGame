﻿using UnityEngine;
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
	
	public Rectangle( MediumText text )
	{
		height = text.textRect.height;
		width = text.textRect.width;
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
		return y;
	}
	
	public float bottom()
	{
		return y + height;
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
		
		if ( ( ( r.right() >= left() && r.right() <= right() )))// || (r.left() >= left () && r.left() <= right())))//horizontally
			//&& (( r.top () >= bottom() && r.top() <= top() )  || ( r.bottom () >= bottom() && r.bottom() <= top() ) ) )//vertically
			return true;
		else return false;
	}

}
