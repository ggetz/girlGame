using UnityEngine;
using System.Collections;

public class ChangeGirlPositionWord : SpecialWords {
	
	Vector2 targetLocation;
	Girl targetGirl;
	public ChangeGirlPositionWord(string font, string word, float scale, Girl girl):base(font, word, scale)
	{
		targetGirl=girl;
	}
	
	public ChangeGirlPositionWord(string font, string word, float scale, Vector2 location, Girl girl): base(font, word, scale)
	{
		targetLocation = location;
		targetGirl = girl;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void setLocation(Vector2 loc)
	{
		targetLocation=loc;
	}
	
	public override void action()
	{
		targetGirl.SetPosition (targetLocation);
	}
	
}
