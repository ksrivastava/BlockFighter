using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public enum Character {Elf, Human, Troll, Orc, Skeleton, Pig};

public class CharacterSelectMenuController : MonoBehaviour {

	private static int NUM_CHARACTERS = 6;

	public Dictionary<int, Character> characters;
	public Dictionary<int, Character> selected;
	public GameObject[] players;
	public GameObject[] markers;
	public Sprite[] sprites;
	public int numPlayers;
	
	void Start () {
		characters = new Dictionary<int, Character>();
		selected = new Dictionary<int, Character>();
		numPlayers = 4;
		for (int i = 0; i < numPlayers; ++i) {
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
		}

		markers[3].SetActive(true);
		selected[3] = Character.Orc;

		if (selected.Keys.Count == numPlayers) {
			GameController.chars = selected;
			MenuController.menu = MenuController.Menu.MapSelection;
			Application.LoadLevel (0);
		}

	}
}
