using UnityEngine;
using System.Collections;

public class ChangeWord : SpecialWords {
	
	bool solid = true;
	public ChangeWord(string font, string word): base(font, word)
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
		solid=false;
	}
	
	
	public void changeBack()
	{
		solid=true;
	}
}
