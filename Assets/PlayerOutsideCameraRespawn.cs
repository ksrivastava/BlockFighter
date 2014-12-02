using UnityEngine;
using System.Collections;

public class PlayerOutsideCameraRespawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var player in PlayerEvents.GetAllPlayers()) {
			if(!player.GetComponentInChildren<PlayerBehavior>().gameObject.renderer.isVisible){
				player.GetComponentInChildren<PlayerBehavior>().Die();
			}
		}
	}
}
