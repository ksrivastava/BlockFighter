using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	private enum State {idle, pickedUp, thrown};


	State state;

	GameObject player = null;
	PlayerControl controller;
	PlayerBehavior behaviour;

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
		print (this.state);
		if ( this.state == State.pickedUp) {

			this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {

			if (this.state == State.idle) {

				player = col.gameObject;
				controller = GameObject.Find (col.transform.parent.name + "/Body").GetComponent<PlayerControl> ();
				behaviour = GameObject.Find (col.transform.parent.name + "/Body").GetComponent<PlayerBehavior> ();
				if(controller != null && controller.pickedUpObject){
					return;
				}


				this.state = State.pickedUp;
				behaviour.weapon = this.gameObject;

				controller.pickedUpObject = true;
				this.collider2D.enabled = false;
				this.rigidbody2D.isKinematic = true;


			} else if (this.state == State.thrown) {
				Damage(col);
			}
		}

		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			if(this.transform.position.y > col.transform.position.y){
				this.collider2D.enabled = true;
			}
		}


	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			this.collider2D.enabled = true;
			controller.pickedUpObject = false;
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

	public void Throw(){
		this.rigidbody2D.isKinematic = false;
		Vector2 rightOrLeft = (controller.facingRight) ? Vector2.right*xMult : Vector2.right*-1*xMult;
		this.rigidbody2D.AddForce(Vector2.up * throwForce + rightOrLeft);
		controller.pickedUpObject = false;
		this.state = State.thrown;
		this.groundCollisions = 0;
	}
}
