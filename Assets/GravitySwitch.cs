using UnityEngine;
using System.Collections;

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
	}

}
