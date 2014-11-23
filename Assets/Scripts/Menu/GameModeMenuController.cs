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
		var inputDevice = InputManager.Devices [0];

		if (Input.GetKeyDown(KeyCode.DownArrow) || (inputDevice != null && inputDevice.DPadDown.WasPressed )) {
			selectedOption = (ModeOption) ((int) (selectedOption + 1) % modeOptions.Length);
		} else if (Input.GetKeyDown(KeyCode.UpArrow) || (inputDevice != null && inputDevice.DPadUp.WasPressed )) {
			int opt = (int) (selectedOption - 1) % modeOptions.Length;
			if (opt < 0) {
				opt += modeOptions.Length;
			}
			selectedOption = (ModeOption) opt;
		} else if (Input.GetKeyDown(KeyCode.Return) || (inputDevice != null && inputDevice.Action1.WasPressed ) ){
			switch (selectedOption) {
				case ModeOption.TimeLimit:
					GameController.mode = new TimeLimitMode(5000);
					Application.LoadLevel (1);
					break;
				case ModeOption.Points:
					GameController.mode = new PointsMode(100);
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
