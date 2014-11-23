using UnityEngine;
using System.Collections;
using InControl;

public class EndGameMenuController : MonoBehaviour {

	private string[] players = {"PlayerOne", "PlayerTwo", "PlayerThree", "PlayerFour"};

	void Start () {
		float[] points = PointsBar.GetAllPoints ();

		Transform pointsTransform = GameObject.Find ("Points").transform;
			for (int i = 0; i < players.Length; ++i) {
			pointsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = points[i].ToString ();
		}
		
		Transform killsTransform = GameObject.Find ("Kills").transform;
		for (int i = 0; i < players.Length; ++i) {
			int numKills = PlayerEvents.GetPlayerStats(players[i]).kills;
			killsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = numKills.ToString();
		}
		
		
		Transform deathsTransform = GameObject.Find ("Deaths").transform;
		for (int i = 0; i < players.Length; ++i) {
			int numDeaths = PlayerEvents.GetPlayerStats(players[i]).deaths;
			deathsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = numDeaths.ToString ();
		}

		Transform trophy = GameObject.Find ("Trophy").transform;
		float highestPoints = -1;
		int idx = -1;
		for (int i = 0; i < points.Length; ++i) {
			if (points[i] > highestPoints) {
				highestPoints = points[i];
				idx = i;
			}
		}

		Vector3 pos = trophy.position;
		pos.x += (idx * 40);
		trophy.position = pos;

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
		}
	}
}
