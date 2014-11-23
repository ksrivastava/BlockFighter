using UnityEngine;
using System.Collections;

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
			killsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = numKills;
		}
		
		
		Transform deathsTransform = GameObject.Find ("Deaths").transform;
		for (int i = 0; i < players.Length; ++i) {
			int numDeaths = PlayerEvents.GetPlayerStats(players[i]).deaths;
			deathsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = numDeaths;
		}

	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
			MenuController.menu = MenuController.Menu.GameModeSelection;
			Application.LoadLevel (0);
		}
	}
}
