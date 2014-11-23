using UnityEngine;
using System.Collections;
using InControl;

public class TitleMenuController : MonoBehaviour {

	public enum TitleOption {Play, Help};
	public GameObject[] titleOptions;
	public GameObject marker;
	
	private TitleOption selectedOption;
	
	void Start () {
		selectedOption = TitleOption.Play;
	}
	
	void Update () {
		var downPressed = false;
		var upPressed = false;
		var actionPressed = false;

		foreach (var device in InputManager.Devices) {
				if (device.DPadDown.WasPressed) {
						downPressed = true;
						break;
				}
	
				if (device.DPadUp.WasPressed) {
						upPressed = true;
						break;
				}
	
				if (device.Action1.WasPressed) {
						actionPressed = true;
						break;
				}
		}



		if (Input.GetKeyDown (KeyCode.DownArrow) || (downPressed)) {
			selectedOption = (TitleOption)((int)(selectedOption + 1) % titleOptions.Length);
		} else if (Input.GetKeyDown (KeyCode.UpArrow) || (upPressed)) {
			int opt = (int)(selectedOption - 1) % titleOptions.Length;
			if (opt < 0) {
					opt += titleOptions.Length;
			}
			selectedOption = (TitleOption)opt;
		} else if (Input.GetKeyDown (KeyCode.Return) || (actionPressed)) {
			switch (selectedOption) {
			case TitleOption.Play:
					MenuController.menu = MenuController.Menu.GameModeSelection;
					Application.LoadLevel (0);
					break;
			case TitleOption.Help:
					MenuController.menu = MenuController.Menu.Help;
					Application.LoadLevel (0);
					break;
			}
		}

		
		Vector3 pos = marker.transform.position;
		pos.y = titleOptions [(int)selectedOption].transform.position.y - 5;
		marker.transform.position = pos;
	}
}
