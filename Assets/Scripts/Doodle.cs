using UnityEngine;
using System.Collections;
using Spine;

public class Doodle : GSpineSprite
{
	public float height, width;
	public Rectangle doodleRect;
	
	public Doodle(string atlas, float h, float w, float sc): base(atlas)
	{
		height=h*sc;
		width=w*sc;
		scale=sc;
	}
	
	// Use this for initialization
	public void Start () 
	{
		doodleRect=new Rectangle(x-width/2f, y-height/2f, width, height);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void Collect()
	{
		Play("Collected", false);
		//GET COLLECTED
		doodleRect = new Rectangle(x, y, 0, 0);
	}
}
