using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	private float health = 100;
	public float Health {
		get {
			print (transform.parent.name + ": " + health.ToString());
			return health;
		}

		set {
			health = value;

			if (health == 0) {
				Destroy(gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Hammer") {
			Health -= 10;
		}
	}
}
