using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public enum Character {Elf, Human, Troll, Orc, Skeleton, Pig};

public class CharacterSelectMenuController : MonoBehaviour {

	private static int NUM_CHARACTERS = 6;

	public Dictionary<int, Character> characters;
	public Dictionary<int, Character> selected;
	public GameObject[] names;
	public GameObject[] players;
	public GameObject[] markers;
	public GameObject[] labels;
	public GameObject[] text;
	public Sprite[] sprites;


	bool isDebugMode = false;

	void Start () {
		characters = new Dictionary<int, Character>();
		selected = new Dictionary<int, Character>();
		for (int i = 0; i < InputManager.Devices.Count; ++i) {
			characters.Add(i, (Character) i);
			markers[i].SetActive(false);
		}
	}
	
	void Update () {
		
		int deviceId = -1;

		var leftPressed = false;
		var rightPressed = false;
		var actionPressed = false;
		var returnPressed = false;


		// keyboard debug
//		deviceId = 0;
//		if (Input.GetKeyDown (KeyCode.LeftArrow) && !selected.ContainsKey(deviceId)) {
//			leftPressed = true;
//		} else if (Input.GetKeyDown (KeyCode.RightArrow) && !selected.ContainsKey(deviceId)) {
//			rightPressed = true;
//		} else if (Input.GetKeyDown (KeyCode.Return) && !selected.ContainsKey(deviceId)) {
//			actionPressed = true;
//		} else if (Input.GetKeyDown (KeyCode.Escape)) {
//			returnPressed = true;
//		}

		for (int i = 0; i < InputManager.Devices.Count; ++i) {
			var device = InputManager.Devices[i];
			deviceId = i;
			if (device.Direction.Left.WasPressed && !selected.ContainsKey(deviceId)) {
				leftPressed = true;
				break;
			} else if(device.Direction.Right.WasPressed && !selected.ContainsKey(deviceId)) {
				rightPressed = true;
				break;
			} else if (device.Action1.WasPressed && !selected.ContainsKey(deviceId)) {
				actionPressed = true;
				break;
			} else if (device.Action2.WasPressed) {
				returnPressed = true;
				break;
			} else if (device.MenuWasPressed) {
				if (selected.Keys.Count == InputManager.Devices.Count && selected.Keys.Count > 1) {
					GameController.chars = selected;
					MenuController.menu = MenuController.Menu.MapSelection;
					Application.LoadLevel (0);
				}
				break;
			}
		}

		if (deviceId != -1) {
			if (rightPressed) {
				characters[deviceId] = (Character) ((int) (characters[deviceId] + 1) % NUM_CHARACTERS);
			} else if (leftPressed) {
				int opt = (int) (characters[deviceId] - 1) % NUM_CHARACTERS;
				if (opt < 0) {
					opt += NUM_CHARACTERS;
				}
				characters[deviceId] = (Character) opt;
			} else if (actionPressed) {
				if (!selected.ContainsValue (characters[deviceId])) {
					//Debug.Log (deviceId);
					markers[deviceId].SetActive(true);
					selected.Add (deviceId, characters[deviceId]);
				}
			} else if (returnPressed) {
				if (selected.ContainsKey (deviceId)) {
					selected.Remove(deviceId);
					markers[deviceId].SetActive(false);
				}
			}
		}

		for (int i = 0; i < players.Length; ++i) {
			if (i < InputManager.Devices.Count) {
				if (!characters.ContainsKey(i)) {
					characters.Add (i, (Character) i);
				}
				names[i].GetComponent<TextMesh>().text = characters[i].ToString();
				players[i].GetComponent<SpriteRenderer>().sprite = sprites[(int) characters[i]];
				if (selected.ContainsValue (characters[i]) && !selected.ContainsKey (i)) {
					Color c = players[i].GetComponent<SpriteRenderer>().color;
					c.a = 0.2f;
					players[i].GetComponent<SpriteRenderer>().color = c;
				} else {
					Color c = players[i].GetComponent<SpriteRenderer>().color;
					c.a = 1.0f;
					players[i].GetComponent<SpriteRenderer>().color = c;
				}
				Color labelC = labels[i].GetComponent<TextMesh>().color;
				labelC.a = 1.0f;
				labels[i].GetComponent<TextMesh>().color = labelC;
				names[i].GetComponent<TextMesh>().color = labelC;
			} else {
				// player not connected
				// dim sprite
				Color playerC = players[i].GetComponent<SpriteRenderer>().color;
				playerC.a = 0.2f;
				players[i].GetComponent<SpriteRenderer>().color = playerC;

				// dim label
				Color labelC = labels[i].GetComponent<TextMesh>().color;
				labelC.a = 0.2f;
				labels[i].GetComponent<TextMesh>().color = labelC;
				names[i].GetComponent<TextMesh>().color = labelC;
				markers[i].SetActive(false);
			}
		}

////		// Debug
		if (isDebugMode) {
			markers[0].SetActive(true);
			selected[0] = Character.Human;
			
			markers[1].SetActive(true);
			selected[1] = Character.Troll;
			
			markers[2].SetActive(true);
			selected[2] = Character.Orc;
			
			markers[3].SetActive(true);
			selected[3] = Character.Pig;

			GameController.chars = selected;
			MenuController.menu = MenuController.Menu.MapSelection;
			Application.LoadLevel (0);
		}

		if (InputManager.Devices.Count < 2) {
			text[0].GetComponent<TextMesh>().text = "Minimum 2 players";
			text[1].GetComponent<TextMesh>().text = "";
		} else if (selected.Keys.Count == InputManager.Devices.Count) {
			text[0].GetComponent<TextMesh>().text = "Press Start To Continue";
			text[1].GetComponent<TextMesh>().text = "";
		} else {
			text[0].GetComponent<TextMesh>().text = "Left/Right to choose";
			text[1].GetComponent<TextMesh>().text = "A/B to select and deselect";
		}

	}
}
