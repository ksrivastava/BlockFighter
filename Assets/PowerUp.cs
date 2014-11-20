using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "Player") {
			OnCollisionWithPlayerBody(col.gameObject);
		}
	}

	protected virtual void OnCollisionWithPlayerBody(GameObject player) {}

	public void KnockBack(Vector3 hitterPosition){
		
		//		print ("KnockBack! " + this.transform.parent.name);
		float knockForce = 300f;
		float upForce = 300f;
		
		rigidbody2D.velocity = Vector2.zero;
		
		//print (hitterPosition.x + "  " + this.transform.position.x);
		if (hitterPosition.x > this.transform.position.x) {
			// hitter is on the right
			rigidbody2D.AddForce(new Vector2(-knockForce,upForce));
		} else {
			// hitter is on the left
			rigidbody2D.AddForce(new Vector2(knockForce,upForce));
		}
	}
}
