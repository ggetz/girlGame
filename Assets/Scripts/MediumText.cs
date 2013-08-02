using UnityEngine;
using System.Collections;

public class MediumText : FLabel {
	
	bool solid=true;
	public MediumText(string font, string text):base(font, text)
	{
		
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool isSolid()
	{
		return solid;
	}
}
