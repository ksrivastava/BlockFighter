using UnityEngine;
using System.Collections;

public class MarkerDie : MonoBehaviour {

	int health; 

	void Start(){
		health = Random.Range(0,100);
	}
	// Update is called once per frame
	void Update () {
		if (health == 0) {
			Destroy(this.gameObject);
		}
		this.health--;
	}
}
