using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
	
	public GameObject weapon = null;
	PlayerControl controller = null;
	public HealthBar healthBar;
	private int playerNum;


	public bool active = true;

	// Use this for initialization
	void Start () {
		controller = GetComponent<PlayerControl> ();
		healthBar = GetComponent<HealthBar> ();
		playerNum = controller.GetPlayerNum ();
		weapon = GameObject.Find (transform.parent.name + "/Hammer");
		controller.pickedUpObject = false;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Fire" + playerNum) > 0){
			if(controller.pickedUpObject){
				ThrowableObject t = weapon.GetComponent<ThrowableObject>();
				t.Throw();
				weapon = GameObject.Find (transform.parent.name+"/Hammer");
			} else {
				HammerControl h = weapon.GetComponent<HammerControl>();
				h.Hit();
			}
		}
	}

	public void ReduceHealth(int n) {
	//	print (playerNum + " got hit");

		healthBar.Health -= n;
		if (healthBar.Health <= 0) {
			this.Die();
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Hammer") {
			var hammerController = col.gameObject.transform.parent.GetComponent<HammerControl>();
			if (hammerController.isHitting && !hammerController.attackComplete) {
				// TELL THE PLAYER EVENT CHECKER THAT YOU HAVE BEEN HIT
				PlayerEvents.RecordAttack(transform.parent.gameObject,col.transform.parent.transform.parent.gameObject,10);

				// do push back stuff


				col.gameObject.collider2D.enabled = false;

				//if the guy who hit you is a teammate, don't take any damage.
				this.TakeHitAction(col.transform.parent.parent.GetChild(0).position);

				//TAKE DAMAGE

				if(!PlayerEvents.FriendlyFireOn && PlayerEvents.GetPlayerStats(col.transform.parent.transform.parent.gameObject.name).isTeammate(transform.parent.gameObject.name)){
					//print("Hit by a teammate!");

					// do no damage
				} else {
						ReduceHealth(10);
				}
			}
		}
	}

	void TakeHitAction(Vector3 hitterPosition){

		print ("KnockBack! " + this.transform.parent.name);
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

		this.gameObject.GetComponent<PlayerControl> ().enabled = false;
		Invoke ("EnablePlayerControl", 1f);

	}

	void EnablePlayerControl(){
		this.gameObject.GetComponent<PlayerControl> ().enabled = true;
	}

	void Die(){
		PlayerEvents.RecordDeath(this.transform.parent.gameObject);
		var respawnPoint = GameObject.Find ("RespawnPoint");
		transform.position = respawnPoint.transform.position;
		this.gameObject.GetComponent<ColorSetter>().ResetColor ();
		PlayerEvents.RemovePlayerFromTeam (this.transform.parent.name);
		healthBar.Health = healthBar.MaxHealth;

		if (this.controller.pickedUpObject) {
			weapon.GetComponent<ThrowableObject>().Drop ();
			weapon = GameObject.Find (transform.parent.name+"/Hammer");
		}

		MakePlayerInactive ();
		Invoke ("MakePlayerActive", 1.0f);
	}



	void MakePlayerInactive(){
		PlayerActiveSetter (false);
	}

	void MakePlayerActive(){
		PlayerActiveSetter (true);
	}

	public void TurnCollidersOff(){
		ColliderSetter (false);
	}

	void TurnCollidersOn(){
		ColliderSetter (true);
	}

	void ColliderSetter(bool value){
		foreach (var collider in transform.parent.GetComponentsInChildren<Collider2D>()) {
			collider.enabled = value;
		}
	}

	
	void PlayerActiveSetter(bool value){
		//state variable
		this.active = value;
		
		// scripts
		this.GetComponent<PlayerControl> ().enabled = value;
		this.GetComponent<HealthBar> ().enabled = value;
		
		// renderers
		this.renderer.enabled = value;
		this.GetComponentInChildren<GUIText> ().enabled = value;
		this.transform.parent.GetComponentInChildren<HammerControl> ().transform.GetChild (0).renderer.enabled = value;
		this.transform.parent.GetComponentInChildren<HammerControl> ().transform.GetChild (1).renderer.enabled = value;
		
		// physics. If it is kinematic then unity physics don't apply, else they do. THATS WHY IT IS SET TO !VALUE. DON'T CHANGE
		this.gameObject.rigidbody2D.isKinematic = !value;
	}
}
