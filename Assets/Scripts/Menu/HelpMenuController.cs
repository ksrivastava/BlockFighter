using UnityEngine;
using System.Collections;
using InControl;

public class HelpMenuController : MonoBehaviour {

	void Update () {
		var returnPressed = false;
		
		foreach (var device in InputManager.Devices) {
			if (device.Action2.WasPressed) {
				returnPressed = true;
				break;
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Backspace) || (returnPressed)) {
			MenuController.menu = MenuController.Menu.Title;
			Application.LoadLevel (0);
		}
	}
}
