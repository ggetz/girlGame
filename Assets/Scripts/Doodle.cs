using UnityEngine;
using System.Collections;
using Spine;

public class Doodle : GSpineSprite
{
	public float height, width;
	
	public Doodle(string atlas, float h, float w, float sc): base(atlas)
	{
		height=h*sc;
		width=w*sc;
		scale=sc;
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void Collect()
	{
		Play("Collected");
		
	}
}
