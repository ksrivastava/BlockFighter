using UnityEngine;
using System.Collections;

public class MarkerDie : MonoBehaviour {

	int health; 

	void Start(){
		health = Random.Range(100,200);
	}
	// Update is called once per frame
	void Update () {
		if (health == 0) {
			Destroy(this.gameObject);
		}
		this.health--;
	}
}
