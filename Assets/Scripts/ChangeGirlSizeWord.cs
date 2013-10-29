using UnityEngine;
using System.Collections;

public class ChangeGirlSizeWord : SpecialWords {
	
	float scaleSize;
	Girl targetGirl;

	public ChangeGirlSizeWord(string font, string word, float textScale, float scale, Girl girl): base(font, word, textScale)
	{
		scaleSize = scale;
		targetGirl = girl;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void action()
	{
		if(scaleSize>targetGirl.scale)
		{
			scaleSize=-scaleSize;
			targetGirl.scale=scaleSize;
			scaleSize=-scaleSize;
		}
		targetGirl.scale=scaleSize;
		targetGirl.changeSize(scaleSize);
	}
		
}
