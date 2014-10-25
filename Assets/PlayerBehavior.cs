using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	public float MaxHealth = 100;
	private float health = 100;
	public GameObject weapon = null;
	PlayerControl controller = null;
	public float Health {
		get {
//			print (transform.parent.name + ": " + health.ToString());
			return health;
		}

		set {
			health = value;

			if (health == 0) {
				Destroy(this.transform.parent.gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {
		controller = GetComponent<PlayerControl> ();
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
		Health -= n;
		print (transform.parent.name + ": " + health.ToString());
	}
	
	void OnTriggerEnter2D(Collider2D col) {
//		if (col.gameObject.tag == "Hammer") {
//			Health -= 10;
//		}
	}
}
