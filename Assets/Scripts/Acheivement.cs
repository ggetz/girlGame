using UnityEngine;
using System.Collections;

public class Acheivement {

	public string title;
	public string desc;
	
	private bool complete;
	
	public Acheivement(string t, string d)
	{
		title = t;
		desc = d;
		complete = false;
	}
	
	public void Complete()
	{
		complete = true;	
	}
	
	public bool isComplete()
	{
		return complete;	
	}
}
