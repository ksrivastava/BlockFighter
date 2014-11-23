using UnityEngine;
using System.Collections;
using InControl;

public class GameModeMenuController : MonoBehaviour {

	public enum ModeOption {TimeLimit, Points, Lives};
	public GameObject[] modeOptions;
	public GameObject marker;

	private ModeOption selectedOption;

	void Start () {
		selectedOption = ModeOption.TimeLimit;
	}

	void Update () {


		var downPressed = false;
		var upPressed = false;
		var actionPressed = false;
		
		foreach (var device in InputManager.Devices) {
			if(device.DPadDown.WasPressed){
				downPressed = true;
				break;
			}

			if(device.DPadUp.WasPressed){
				upPressed = true;
				break;
			}

			if(device.Action1.WasPressed){
				actionPressed = true;
				break;
			}
		}



		if (Input.GetKeyDown(KeyCode.DownArrow) || (downPressed)) {
			selectedOption = (ModeOption) ((int) (selectedOption + 1) % modeOptions.Length);
		} else if (Input.GetKeyDown(KeyCode.UpArrow) || (upPressed)) {
			int opt = (int) (selectedOption - 1) % modeOptions.Length;
			if (opt < 0) {
				opt += modeOptions.Length;
			}
			selectedOption = (ModeOption) opt;
		} else if (Input.GetKeyDown(KeyCode.Return) || (actionPressed ) ){
			switch (selectedOption) {
				case ModeOption.TimeLimit:
					GameController.mode = new TimeLimitMode(10000);
					Application.LoadLevel (1);
					break;
				case ModeOption.Points:
					GameController.mode = new PointsMode(1000);
					Application.LoadLevel (1);
					break;
				case ModeOption.Lives:
					GameController.mode = new LivesMode();
					Application.LoadLevel (1);
					break;
			}
		}

		Vector3 pos = marker.transform.position;
		pos.y = modeOptions [(int) selectedOption].transform.position.y - 5;
		marker.transform.position = pos;
	}
}
