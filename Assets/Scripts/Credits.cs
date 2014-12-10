using UnityEngine;
using System.Collections;
using InControl;

public class Credits : MonoBehaviour {

	Vector2 topRight;
	// Use this for initialization
	void Start () {
		topRight = this.camera.ViewportToWorldPoint (new Vector2 (1, 1));
	}
	
	// Update is called once per frame
	void Update () {

		var endMarker = GameObject.Find ("EndOfCreditsMarker");

		if (endMarker != null) {
			if(endMarker.transform.position.y >= topRight.y){
				MenuController.menu = MenuController.Menu.Title;
				Application.LoadLevel (0);
			}
		}

		bool buttonPressed = false;

		foreach (var device in InputManager.Devices) {
			if (device.Action2.WasPressed || device.MenuWasPressed) {
				buttonPressed = true;
				break;
			} 
		}


		if (buttonPressed || Input.GetKeyDown(KeyCode.Escape)) {
			MenuController.menu = MenuController.Menu.Title;
			Application.LoadLevel (0);
		}


		for(int i=0; i< transform.childCount ; i++) {
			var child = transform.GetChild(i);
			var pos = child.position;
			pos.y += 0.1f;
			child.position = pos;
		}


	}
}
