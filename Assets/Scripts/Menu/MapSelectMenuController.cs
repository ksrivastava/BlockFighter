using UnityEngine;
using System.Collections;
using InControl;

public class MapSelectMenuController : MonoBehaviour {

	public enum GameMap {Map1, Map2};
	public GameObject[] mapOptions;
	public GameObject marker;
	
	private GameMap selectedOption;
	
	void Start () {
		selectedOption = GameMap.Map1;
	}
	
	void Update () {
		
		
		var leftPressed = false;
		var rightPressed = false;
		var actionPressed = false;
		var returnPressed = false;
		
		foreach (var device in InputManager.Devices) {
			if (device.Direction.Left.WasPressed) {
				leftPressed = true;
				break;
			} else if(device.Direction.Right.WasPressed) {
				rightPressed = true;
				break;
			} else if (device.Action1.WasPressed) {
				actionPressed = true;
				break;
			} else if (device.Action2.WasPressed) {
				returnPressed = true;
				break;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.RightArrow) || (rightPressed)) {
			selectedOption = (GameMap) ((int) (selectedOption + 1) % mapOptions.Length);
		} else if (Input.GetKeyDown(KeyCode.LeftArrow) || (leftPressed)) {
			int opt = (int) (selectedOption - 1) % mapOptions.Length;
			if (opt < 0) {
				opt += mapOptions.Length;
			}
			selectedOption = (GameMap) opt;
		} else if (Input.GetKeyDown(KeyCode.Return) || (actionPressed ) ){
			switch (selectedOption) {
			case GameMap.Map1:
				Application.LoadLevel (1);
				break;
			case GameMap.Map2:
				Application.LoadLevel (2);
				break;
			}
		} else if (Input.GetKeyDown (KeyCode.Escape) || (returnPressed)) {
			MenuController.menu = MenuController.Menu.GameModeSelection;
			Application.LoadLevel (0);
		}

		for (int i = 0; i < mapOptions.Length; ++i) {
			mapOptions [i].renderer.material.shader = Shader.Find ("Diffuse");
		}

		mapOptions [(int)selectedOption].renderer.material.shader = Shader.Find ("Unlit/Texture");
	}
}
