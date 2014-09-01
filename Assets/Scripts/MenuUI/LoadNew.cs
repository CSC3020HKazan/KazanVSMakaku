using UnityEngine;
using System.Collections;

public class LoadNew : MonoBehaviour {
	
	private bool mouseOver = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			if (mouseOver) {
				Application.LoadLevel(1);
			}
		}
		
	}
	
	void OnMouseEnter() {
		mouseOver = true;
	 	guiText.fontSize+= 10;
	}
	
	void OnMouseExit() {
		mouseOver = false;
		guiText.fontSize -= 10;
	}
}
