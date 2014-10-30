using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
	
	public GameObject weapon = null;
	GameObject pointLight = null;
	PlayerControl controller = null;
	HealthBar healthBar;

	// Use this for initialization
	void Start () {
		controller = GetComponent<PlayerControl> ();
		healthBar = GetComponent<HealthBar> ();
		weapon = GameObject.Find (transform.parent.name+"/Hammer");
		pointLight = Instantiate (Resources.Load ("PointLight")) as GameObject;
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

		// Move the point light with player
		Vector3 pos = pointLight.transform.position;
		pos.x = transform.position.x;
		pos.y = transform.position.y;
		pointLight.transform.position = pos;
	}

	public void ToggleLight(bool on) {
		pointLight.SetActive (on);
	}

	public void ReduceHealth(int n) {
		healthBar.Health -= n;
		if (healthBar.Health <= 0) {
			Destroy(this.transform.parent.gameObject);
		}
	}
}
