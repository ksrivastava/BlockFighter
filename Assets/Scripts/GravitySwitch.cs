using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class GravitySwitch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Hammer") {
			Flip();
		}
	}

	void Flip() {
		var g = Physics2D.gravity;
		g.y *= -1;
		Physics2D.gravity = g;

//		var Players = GameObject.FindGameObjectsWithTag ("Player");
//
//		foreach(GameObject player in Players) {
//			var playerPoint = player.transform.parent;
//			playerPoint.transform.Rotate(new Vector3(0, 0, 180));
////			var rot = playerPoint.transform.rotation;
////
////			playerPoint.transform.rotation = rot;	                                 
//		}
	}
}
