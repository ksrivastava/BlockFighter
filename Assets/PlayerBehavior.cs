using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	public float MaxHealth = 100;
	private float health = 100;
	public float Health {
		get {
//			print (transform.parent.name + ": " + health.ToString());
			return health;
		}

		set {
			health = value;

			if (health <= 0) {
				Destroy(this.transform.parent.gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReduceHealth(int n) {
		Health -= n;
		print (transform.parent.name + ": " + health.ToString());
	}
	
	void OnTriggerEnter2D(Collider2D col) {
//		if (col.gameObject.tag == "Hammer") {
//			Health -= 10;
//		}
	}
}
