using UnityEngine;
using System.Collections;

public class Scroll : MovingPictureObstacles {

	float scale;
	public float height = 90;
	public float width = 450;

	public Scroll(string atlas, float x):base(atlas)
	{
		scale = x;
			
	}

	// Use this for initialization
	public void Start () 
	{
		height = height*scale;
		width = width*scale;
		Play ("Still");	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void action()
	{
		Play("Roll Out", false);
		
	}
}
