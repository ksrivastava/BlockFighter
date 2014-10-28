using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	public enum State {idle, pickedUp, thrown};


	protected State state;

	public GameObject player = null;
	PlayerControl controller;
	PlayerBehavior behaviour;

	protected float throwForce = 1000f;
	protected float xMult= 500f;

	private int groundCollisionsBeforeIdle = 10;
	private Vector2 velocityBeforeIdle = new Vector2(3f,3f);
	private int groundCollisions = 0;
	private Vector2 displacement = new Vector2(2,2);


	// this is what you override to implement damage and things.
	public virtual void Damage(Collider2D col){}

	// Use this for initialization
	protected void Start () {
		this.state = State.idle;
	}
	
	// Update is called once per frame
	void Update () {
//		print (this.state);
		if ( this.state == State.pickedUp) {

			this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
			var pos = this.transform.position;
			pos.x  = (controller.facingRight) ? pos.x + displacement.x : pos.x - displacement.x;
			pos.y += displacement.y;
			this.transform.position = pos;
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
		Vector2 rightOrLeft = (controller.facingRight) ? Vector2.right*xMult : Vector2.right*-1*xMult;
		this.rigidbody2D.AddForce(Vector2.up * throwForce + rightOrLeft);

		controller.pickedUpObject = false;
		this.state = State.thrown;
		this.collider2D.enabled = true;
		this.rigidbody2D.velocity = Vector2.zero;
		//this.rigidbody2D.isKinematic = false;
		this.groundCollisions = 0;
	}
}
