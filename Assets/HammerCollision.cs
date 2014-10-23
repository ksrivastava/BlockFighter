using UnityEngine;
using System.Collections;

public class HammerCollision : MonoBehaviour {

	HammerControl controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		print (col.gameObject.name);
	}
	
}
