using UnityEngine;
using System.Collections;

public class ChangeWord : SpecialWords {
	
	bool isSolid = true;
	public ChangeWord(string font, string word, float scale): base(font, word, scale)
	{
		
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void action()
	{
		Debug.Log("yo I worked");
		isSolid=false;
	}
	
	
	public void changeBack()
	{
		isSolid=true;
	}
	
	
}
