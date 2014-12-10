using UnityEngine;
using System.Collections;
using InControl;

public class HelpMenuController : MonoBehaviour {

	public GameObject[] screens;
	int screenIdx = 0;

	void Start() {
		SetActiveScreen (screenIdx);
	}

	void Update () {
		var returnPressed = false;
		
		foreach (var device in InputManager.Devices) {
			if (device.Action2.WasPressed) {
				returnPressed = true;
				break;
			}

			if (device.Action1.WasPressed) {
				screenIdx = Mathf.Min (screenIdx + 1, screens.Length - 1);
				SetActiveScreen (screenIdx);
				break;
			}
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			screenIdx = Mathf.Min (screenIdx + 1, screens.Length - 1);
			SetActiveScreen (screenIdx);
		}

		else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			screenIdx = Mathf.Max (screenIdx - 1, 0);
			SetActiveScreen (screenIdx);
		}

		else if (Input.GetKeyDown (KeyCode.Escape) || (returnPressed)) {
			MenuController.menu = MenuController.Menu.Title;
			Application.LoadLevel (0);
		}
	}

	void SetActiveScreen(int idx) {
		foreach (GameObject screen in screens) {
			screen.SetActive(false);
		}
		screens [idx].SetActive (true);
	}
}
