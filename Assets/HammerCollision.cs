using UnityEngine;
using System.Collections;

public class HammerCollision : MonoBehaviour {

	HammerControl controller;

	// Use this for initialization
	void Start () {
		controller = this.transform.parent.gameObject.GetComponent<HammerControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col) {
		print (col.gameObject.name);
		if (col.gameObject.tag.Contains("Player")) {
			if (controller.isHitting && !controller.attackComplete) {
				col.gameObject.GetComponent<PlayerBehavior>().ReduceHealth(10);
			}
		}
	}
	
}
