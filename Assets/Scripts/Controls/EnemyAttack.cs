using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	void Start () {
	
	}

	void Update () {
	
	}

	void FixedUpdate() {
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag.Contains ("Player")) {
			col.gameObject.GetComponent<PlayerBehavior>().ReduceHealth(10);
			Destroy (gameObject);
		}

		if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			Destroy (gameObject);
		}
	}

	public void Launch (Vector2 force) {
		rigidbody2D.AddForce (force);
	}
}
