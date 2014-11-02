using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	public enum State {idle, pickedUp, thrown};


	protected State state;

	public GameObject hammer = null;
	PlayerControl controller;
	PlayerBehavior behaviour;

	protected float throwForce = 1000f;
	protected float xMult= 500f;
	public bool canPickUp = true;

	private int groundCollisionsBeforeIdle = 10;
	private Vector2 velocityBeforeIdle = new Vector2(3f,3f);
	private int groundCollisions = 0;
	private Vector2 displacement = new Vector2(1.1f,0);


	// this is what you override to implement damage and things.
	public virtual void Damage(Collider2D col){}

	// Use this for initialization
	protected void Start () {
		this.state = State.idle;
	}
	
	// Update is called once per frame
	void Update () {
//		print (this.state);
		if (this.state == State.pickedUp) {
			this.rigidbody2D.velocity = Vector2.zero;
			var pos = new Vector3(hammer.transform.position.x, hammer.transform.position.y, this.transform.position.z);
			pos.x  = (controller.facingRight) ? pos.x + displacement.x : pos.x - displacement.x;
//			pos.z = pos.z + 1;
			this.transform.position = pos;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {

		if (col.gameObject.tag == "Player") {

			if (this.state == State.idle && this.canPickUp) {

//				player = col.gameObject;
				hammer = GameObject.Find (col.transform.parent.name + "/Hammer/Body");

				controller = GameObject.Find (col.transform.parent.name + "/Body").GetComponent<PlayerControl> ();
				behaviour = GameObject.Find (col.transform.parent.name + "/Body").GetComponent<PlayerBehavior> ();
				if(controller != null && controller.pickedUpObject){
					return;
				}

				this.transform.parent = hammer.transform;

				this.state = State.pickedUp;

				behaviour.weapon = this.gameObject;
				controller.pickedUpObject = true;
				this.collider2D.enabled = false;
				//this.rigidbody2D.isKinematic = true;
			} else if (this.state == State.thrown) {
				Damage(col);
				this.rigidbody2D.velocity = Vector2.zero;
				this.state = State.idle;
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

	public void Throw(){
		this.transform.parent = null;
		Vector2 rightOrLeft = (controller.facingRight) ? Vector2.right*xMult : Vector2.right*-1*xMult;

		var t = (Physics2D.gravity.y > 0)? -throwForce : throwForce;

		this.rigidbody2D.AddForce(Vector2.up * t + rightOrLeft);

		controller.pickedUpObject = false;
		this.state = State.thrown;
		this.collider2D.enabled = true;
		this.rigidbody2D.velocity = Vector2.zero;
		//this.rigidbody2D.isKinematic = false;
		this.groundCollisions = 0;
	}
}
