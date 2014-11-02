using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class GravitySwitch : MonoBehaviour {

	public bool isFlipping = false;

	// Use this for initialization
	void Start () {
//		if (isFlipping) Invoke ("Flip", 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Hammer") {
			var hammerController = col.gameObject.transform.parent.GetComponent<HammerControl>();
			if (hammerController.isHitting && !hammerController.attackComplete) {
				Flip();
				col.gameObject.collider2D.enabled = false;
			}
		}
	}

	void Flip() {
		print ("Gravity flipping");
		var g = Physics2D.gravity;
		g.y *= -1;
		Physics2D.gravity = g;

		var Players = GameObject.FindGameObjectsWithTag ("Player");

		foreach(GameObject player in Players) {
			var name = player.transform.parent.name;
			var Body = GameObject.Find (name + "/Body");
			var Hammer = GameObject.Find (name + "/Hammer");
			Body.transform.Rotate(new Vector3(0, 0, 180));                             
			Hammer.transform.Rotate(new Vector3(0, 0, 180));
		}
	}
}
