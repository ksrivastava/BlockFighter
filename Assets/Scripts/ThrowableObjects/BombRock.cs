using UnityEngine;
using System.Collections;

public class BombRock : StraightRock {

	bool stick = false;
	GameObject stickObject;
	PlayerControl stickPlayerControl;
	PlayerBehavior stickPlayerBehaviour;
	string stickPlayerName;

	float countDown = 4;

	public override void Damage (Collider2D col)
	{
		try{

			stickPlayerName = col.transform.parent.name;


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
		float xForce = 2000;
		float upForce = 2000;//1000;


		stickPlayerControl.allowMovement = false;

		if (stickPlayerControl.facingRight) {
			stickPlayerControl.rigidbody2D.AddForce (new Vector2 (xForce, upForce));
		} else {
			stickPlayerControl.rigidbody2D.AddForce (new Vector2 (-xForce, upForce));
		}

		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Ground"),LayerMask.NameToLayer(GetPlayerLayerName()),true);
		Invoke ("DoDamage", 1f);

	}

	void DoDamage(){
		stickPlayerBehaviour.ReduceHealth (80);
		// turn on stickplayercontrol
		//Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Ground"),LayerMask.NameToLayer(GetPlayerLayerName()),false);
		stickPlayerControl.allowMovement = true;
		Destroy (this.gameObject);
	}

	string GetPlayerLayerName(){
		string playerLayer = "Player";
		
		if (stickPlayerName == "PlayerOne") {
			playerLayer += "1";
		} else if (stickPlayerName == "PlayerTwo") {
			playerLayer += "2";		
		} else if (stickPlayerName == "PlayerThree") {
			playerLayer += "3";
		} else if (stickPlayerName == "PlayerOne") {
			playerLayer += "4";
		}

		return playerLayer;
	}
}
