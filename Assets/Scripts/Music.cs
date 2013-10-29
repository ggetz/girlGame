using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	
	private static bool created = false;
 
	void Awake() {
	    if (!created) {
	        // this is the first instance - make it persist
	        DontDestroyOnLoad(this.gameObject);
	        created = true;
	    } else {
	        // this must be a duplicate from a scene reload - DESTROY!
	        Destroy(this.gameObject);
    	} 
	}
	
	void Update() {
		if (!(Application.loadedLevelName.Equals("MainMenu")||Application.loadedLevelName.Equals("AcheivementScreen")))
			Destroy(this.gameObject);	
	}
}
