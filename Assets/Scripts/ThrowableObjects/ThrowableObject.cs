using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	private enum State {idle, pickedUp, thrown};


	State state;

	GameObject player = null;
	PlayerControl controller;

	protected float throwForce = 1000f;
	protected float xMult= 500f;

	private int groundCollisionsBeforeIdle = 10;
	private Vector2 velocityBeforeIdle = new Vector2(3f,3f);
	private int groundCollisions = 0;
	private Collider2D triggerCollider = null;

	// this is what you override to implement damage and things.
	public virtual void Damage(Collider2D col){}

	// Use this for initialization
	protected void Start () {
		this.state = State.idle;
		
		// turn off the unity physics collider
		var colliders = this.gameObject.GetComponents<Collider2D>();
		foreach(var c in colliders){
			if(c.isTrigger){
				triggerCollider = c;
				break;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		if ( this.state == State.pickedUp) {

			this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);

			string fireInput = controller.isSecondPlayer ? "Fire2" : "Fire1";
			if(Input.GetAxis(fireInput) > 0){
				Throw();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			if (this.state == State.idle) {
				this.state = State.pickedUp;
				this.collider2D.enabled = false;
				this.rigidbody2D.isKinematic = true;
				player = col.gameObject;
				controller = GameObject.Find (col.transform.parent.name + "/Body").GetComponent<PlayerControl> ();


			} else if (this.state == State.thrown) {
				Damage(col);
				this.state = State.idle;
			}
		} else if(col.gameObject.layer == LayerMask.NameToLayer("Ground") && this.rigidbody2D.velocity == Vector2.zero){
			this.state = State.idle;
		}

		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			if(this.transform.position.y > col.transform.position.y){
				this.collider2D.enabled = true;
			}
		}


	}

	void OnCollisionStay2D(Collision2D coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Ground") 
		    && Mathf.Abs(this.rigidbody2D.velocity.x) <= velocityBeforeIdle.x
		    && Mathf.Abs(this.rigidbody2D.velocity.y) <= velocityBeforeIdle.y) {
			if(this.groundCollisions == this.groundCollisionsBeforeIdle){
				this.state = State.idle;
			}
			this.groundCollisions++;
		}
	}

	void Throw(){
		this.rigidbody2D.isKinematic = false;
		Vector2 rightOrLeft = (controller.facingRight) ? Vector2.right*xMult : Vector2.right*-1*xMult;
		this.rigidbody2D.AddForce(Vector2.up * throwForce + rightOrLeft);
		//print ("Thrown!");
		this.state = State.thrown;
		this.groundCollisions = 0;
		Invoke ("TurnTriggerColliderBackOn", 0.05f);
	}

	void TurnTriggerColliderBackOn(){
		triggerCollider.enabled = true;
	}

}
