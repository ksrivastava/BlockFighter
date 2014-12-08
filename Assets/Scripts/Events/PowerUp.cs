using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {


	protected bool idleSinceBirth;
	protected float killTime = 10;
	protected float totalLifetime = 40;
	
	protected virtual void Awake(){
		KillTimer ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.rigidbody2D.velocity.magnitude >= 50) {
			this.rigidbody2D.velocity = Vector2.zero;
		}
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

	
	protected void KillTimer(){
		idleSinceBirth = true;
		
		Invoke ("BlinkForTwoSeconds", killTime - 2);
		Invoke ("CheckAndKill", killTime);
		Invoke ("BlinkForTwoSeconds", totalLifetime - 2);
		Invoke ("Kill", totalLifetime);
	}
	
	void CheckAndKill(){
		if (idleSinceBirth) {
			Destroy(this.gameObject);
		}
	}
	
	void BlinkForTwoSeconds(){
		StartCoroutine(Blink(2));
	}
	
	void Kill(){
		Destroy (this.gameObject);
	}
	
	
	public IEnumerator Blink(float blinkTime) {
		var endTime = Time.time + blinkTime;
		while(Time.time < endTime){
			renderer.enabled = false;
			yield return new WaitForSeconds(0.2f);
			renderer.enabled = true;
			yield return new WaitForSeconds(0.2f);
		}
	}

}
