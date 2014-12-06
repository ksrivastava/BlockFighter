using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	public enum State {idle, pickedUp, thrown};


	public State state;

	public GameObject hammer = null;
	protected PlayerControl controller;
	PlayerBehavior behaviour;

	protected float throwForce = 1000f;
	protected float xMult= 500f;
	protected int damageVal = 0;

	public bool canPickUp = true;

	private int groundCollisionsBeforeIdle = 5;
	private Vector2 velocityBeforeIdle = new Vector2(15f,15f);
	private int groundCollisions = 0;
	private Vector2 displacement = new Vector2(1.1f,0);
	private bool canDamageSelf = false;

	// this is what you override to implement damage and things.
	public virtual void Damage(Collider2D col){}


	// Use this for initialization
	void Start () {
		this.state = State.idle;
		this.collider2D.enabled = true;
	}
	
	// Update is called once per frame
	public virtual void Update () {

		if (this.state == State.pickedUp) {
				this.rigidbody2D.velocity = Vector2.zero;
				var pos = new Vector3 (hammer.transform.position.x, hammer.transform.position.y, 0);
				pos.x = (controller.facingRight) ? pos.x + displacement.x : pos.x - displacement.x;
				this.transform.position = pos;
			this.canDamageSelf = false;
			this.collider2D.enabled = false;
		}

	
		if (this.rigidbody2D.velocity.magnitude >= 50) {
			this.rigidbody2D.velocity = Vector2.zero;
		}
	}

	GameObject getTopParent(GameObject input){
		Transform t = input.transform;
		Transform p = input.transform.parent;
		
		while (p != null) {
			t = p;
			p = p.transform.parent;
		}
		
		return t.gameObject;
	}

	void OnTriggerEnter2D(Collider2D col) {


		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy" || LayerMask.LayerToName(col.gameObject.layer).Contains("Player")) {

			if (this.state == State.idle && this.canPickUp && (col.gameObject.tag == "Player" || LayerMask.LayerToName(col.gameObject.layer).Contains("Player") )) {


				var playerObject = getTopParent(col.gameObject);
				hammer = playerObject.GetComponentInChildren<HammerControl>().gameObject.transform.GetChild(0).gameObject;
				controller = playerObject.GetComponentInChildren<PlayerControl>();
				behaviour = playerObject.GetComponentInChildren<PlayerBehavior>();

				if(controller == null || hammer == null || behaviour == null){
					hammer = null;
					controller = null;
					behaviour = null;
					return;
				}

				if(controller != null && controller.pickedUpObject){
					hammer = null;
					controller = null;
					behaviour = null;
					return;
				} 

				this.state = State.pickedUp;


				this.canDamageSelf = false;
				behaviour.weapon = this.gameObject;
				controller.pickedUpObject = true;
				this.collider2D.enabled = false;
			
			} else if (this.state == State.thrown) {

				
				//find highest parent
				
				Transform playerTransform = null;
				Transform c = col.gameObject.transform;
				
				while(c.transform.parent != null){
					playerTransform = c.transform.parent;
					c = playerTransform;
				}

				// don't damage self

				if( (playerTransform.GetComponentInChildren<PlayerControl>().playerName != controller.playerName) || canDamageSelf){
					Damage(playerTransform.GetComponentInChildren<PlayerBehavior>().gameObject.collider2D);
				}
				this.rigidbody2D.velocity = Vector2.zero;

				this.state = State.idle;
				this.canDamageSelf = false;
				
				// RECORD EVENT WITH PLAYER TRACKER
				if (col.gameObject.tag == "Player") {
					PlayerEvents.RecordAttack(playerTransform.gameObject,controller.transform.parent.gameObject,damageVal);
				}
			}
		} else if(col.gameObject.name == "BouncyWall"){
			canDamageSelf = true;
		}
	}


	void OnCollisionStay2D(Collision2D coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Ground") 
		    || this.rigidbody2D.velocity.magnitude <= velocityBeforeIdle.magnitude) {
			if(this.groundCollisions == this.groundCollisionsBeforeIdle){

				this.state = State.idle;
				this.canDamageSelf = false;
			}
			this.groundCollisions++;
		}
	}

	public virtual void Throw(){
		this.transform.parent = null;
		Vector2 rightOrLeft = (controller.facingRight) ? Vector2.right*xMult : Vector2.right*-1*xMult;
		
		var t = (Physics2D.gravity.y > 0)? -throwForce : throwForce;
		this.rigidbody2D.AddForce(Vector2.up * t + rightOrLeft);
		
		controller.pickedUpObject = false;
		this.state = State.thrown;
		Invoke ("EnableCollider", 0.05f);

		this.rigidbody2D.velocity = Vector2.zero;
		//this.rigidbody2D.isKinematic = false;
		this.groundCollisions = 0;
	}

	public void EnableCollider(){
		this.collider2D.enabled = true;
	}

	public void Drop() {

		this.state = State.idle;
		this.canDamageSelf = false;
		this.collider2D.enabled = true;
		controller.pickedUpObject = false;
	}

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
