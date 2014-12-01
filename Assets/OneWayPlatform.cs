using UnityEngine;
using System.Collections;
/*
 * Some simple code for One Way (cloud) Platforms.
 * USAGE: works with horizontal platforms that only the player will stand on.
 * Platform pivot should be where the player can stand. Player pivot should be at the player's feet.
 * <3 - @x01010111
 */

public class OneWayPlatform : MonoBehaviour {
	
	public string playerName = "PlayerOne";
	private GameObject player;
	
	//Find player by name
	void Start () {
		player = GameObject.Find(playerName+"/Body/groundCheck");
		if (player == null) Debug.LogError("(One Way Platform) Please enter correct player name in Inspector for: " + gameObject.name);
	}

	void SetEnableColliders(bool setEnable) {
		gameObject.collider2D.enabled = setEnable;
		foreach (var col in  gameObject.GetComponentsInChildren<Collider2D> ()) {
			col.enabled = setEnable;
		}
	}

	//Check to see if player is under the platform. Collide only if the player is above the platform.
	void Update () {
		if (player != null) {
			if (player.transform.position.y < this.transform.position.y) {
				SetEnableColliders(false);
			}
			else {
				SetEnableColliders(true);
			}
		}
	}

	void OnTriggerEnter2D() {

	}
}