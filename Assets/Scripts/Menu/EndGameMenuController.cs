using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class EndGameMenuController : MonoBehaviour {

	public GameObject[] spritesObj;
	public Sprite elf, human, troll, orc, skeleton, pig;

	private string[] players = {"PlayerOne", "PlayerTwo", "PlayerThree", "PlayerFour"};
	private Color32[] colors = {new Color32(129,22,159,255), new Color32(255,255,0,255),
		new Color32(37,218,38,255), new Color32(212,45,45,255)};

	void Start () {
		float[] points = PointsBar.GetAllPoints ();

		foreach (int p in GameController.chars.Keys) {
			switch (GameController.chars[p]) {
			case Character.Elf:
				spritesObj[p].GetComponent<SpriteRenderer>().sprite = elf;
				break;
			case Character.Human:
				spritesObj[p].GetComponent<SpriteRenderer>().sprite = human;
				break;
			case Character.Troll:
				spritesObj[p].GetComponent<SpriteRenderer>().sprite = troll;
				break;
			case Character.Orc:
				spritesObj[p].GetComponent<SpriteRenderer>().sprite = orc;
				break;
			case Character.Skeleton:
				spritesObj[p].GetComponent<SpriteRenderer>().sprite = skeleton;
				break;
			case Character.Pig:
				spritesObj[p].GetComponent<SpriteRenderer>().sprite = pig;
				break;
			}
		}

		Transform pointsTransform = GameObject.Find ("Points").transform;
		for (int i = 0; i < PointsBar.numPlayers; ++i) {
			pointsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = points[i].ToString ();
		}
		
		Transform killsTransform = GameObject.Find ("Kills").transform;
		for (int i = 0; i < PointsBar.numPlayers; ++i) {
			int numKills = PlayerEvents.GetPlayerStats(players[i]).kills;
			killsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = numKills.ToString();
		}
		
		
		Transform deathsTransform = GameObject.Find ("Deaths").transform;
		for (int i = 0; i < PointsBar.numPlayers; ++i) {
			int numDeaths = PlayerEvents.GetPlayerStats(players[i]).deaths;
			deathsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = numDeaths.ToString ();
		}

		Transform trophy = GameObject.Find ("Trophy").transform;
		float highestPoints = 0;
		bool tie = true;
		int idx = -1;
		for (int i = 0; i < points.Length; ++i) {
			if (points[i] > highestPoints) {
				highestPoints = points[i];
				idx = i;
				tie = false;
			} else if (points[i] == highestPoints) {
				tie = true;
			}
		}

		if (!tie) {
			Vector3 pos = trophy.position;
			pos.x += (idx * 40);
			trophy.position = pos;
		} else {
			Destroy (trophy.gameObject);
		}

	}

	void Update () {

		var menuPressed = false;

		foreach (var device in InputManager.Devices) {
			if(device.MenuWasPressed){
				menuPressed = true;
				break;
			}
		}

		if (Input.GetKeyDown(KeyCode.Return) || (menuPressed)) {
			MenuController.menu = MenuController.Menu.GameModeSelection;
			Application.LoadLevel (0);
			PointsBar.Clear ();
		}
	}
}
