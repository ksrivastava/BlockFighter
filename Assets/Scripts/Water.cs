using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player") {
			col.gameObject.GetComponent<PlayerBehavior> ().ReduceHealth (500);
		} else if(col.gameObject.name.Contains("Rock")) {
				Destroy(col.gameObject);
		}

	}
}
