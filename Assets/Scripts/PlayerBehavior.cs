using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
	
	public GameObject weapon = null;
	PlayerControl controller = null;
	public HealthBar healthBar;

	// Use this for initialization
	void Start () {
		controller = GetComponent<PlayerControl> ();
		healthBar = GetComponent<HealthBar> ();
		weapon = GameObject.Find (transform.parent.name+"/Hammer");
	}
	
	// Update is called once per frame
	void Update () {
		
		string fireInput = controller.isSecondPlayer ? "Fire2" : "Fire1";
		if(Input.GetAxis(fireInput) > 0){
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
		healthBar.Health -= n;
		if (healthBar.Health <= 0) {
			
			//TELL THE PLAYER EVENT CHECKER THAT YOU HAVE JUST DIED
			PlayerEvents.RecordDeath(this.transform.parent.gameObject);

			Destroy(this.transform.parent.gameObject);

		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Hammer") {
			var hammerController = col.gameObject.transform.parent.GetComponent<HammerControl>();
			if (hammerController.isHitting && !hammerController.attackComplete) {
				ReduceHealth(10);
				col.gameObject.collider2D.enabled = false;

				// TELL THE PLAYER EVENT CHECKER THAT YOU HAVE BEEN HIT
				PlayerEvents.RecordAttack(transform.parent.gameObject,col.transform.parent.transform.parent.gameObject,10);
			}
		}
	}
}
