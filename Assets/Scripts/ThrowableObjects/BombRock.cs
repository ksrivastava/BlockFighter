using UnityEngine;
using System.Collections;

public class BombRock : StraightRock {

	bool stick = false;
	GameObject stickObject;
	PlayerControl stickPlayerControl;
	PlayerBehavior stickPlayerBehaviour;

	float countDown = 4;

	public override void Damage (Collider2D col)
	{
		try{

			var playerName = col.transform.parent.name;
			print(playerName);

			stick=true;
			stickObject = col.transform.gameObject;
			stickPlayerControl = stickObject.GetComponent<PlayerControl>();
			stickPlayerBehaviour =  stickObject.GetComponent<PlayerBehavior>();

			this.rigidbody2D.isKinematic = true;
			foreach (var collider in GetComponents<Collider2D>()){
				collider.enabled = false;
			}
			Invoke ("Explode",countDown);

		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}

	public override void Update(){
		if (stick) {
			if(stickPlayerControl.facingRight){
				transform.position = stickObject.transform.position + new Vector3(-1,0,0);
			} else {
				transform.position = stickObject.transform.position + new Vector3(1,0,0);
			}
		}
		base.Update ();
	}

	void Explode(){
		float xForce = 5000;
		float upForce = 500;
		// turn off stickplayercontrol
		if (stickPlayerControl.facingRight) {
			stickPlayerControl.rigidbody2D.AddForce (new Vector2 (xForce, upForce));
		} else {
			stickPlayerControl.rigidbody2D.AddForce (new Vector2 (-xForce, upForce));
		}


		Invoke ("DoDamage", 0.25f);

	}

	void DoDamage(){
		stickPlayerBehaviour.ReduceHealth (80);
		// turn on stickplayercontrol
		Destroy (this.gameObject);
	}
}
