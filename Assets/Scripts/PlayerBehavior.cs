using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
	
	public GameObject weapon = null;
	PlayerControl controller = null;
	public HealthBar healthBar;
	private int playerNum;

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
		print (playerNum + " got hit");

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
			
				col.gameObject.collider2D.enabled = false;

				//if the guy who hit you is a teammate, don't take any damage.

				if( !PlayerEvents.FriendlyFireOn && PlayerEvents.GetPlayerStats(col.transform.parent.transform.parent.gameObject.name).isTeammate(transform.parent.gameObject.name)){
					//print("Hit by a teammate!");

					// do no damage
				} else {
						ReduceHealth(10);
				}
			}
		}
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

		//TODO: this causes issues with the TeamUp being called when player is inactive
		this.transform.parent.gameObject.SetActive (false);
		Invoke ("ReactivatePlayer", 1.0f);
	}

	void ReactivatePlayer(){
		this.transform.parent.gameObject.SetActive (true);
	}
}
