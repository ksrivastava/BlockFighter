using UnityEngine;
using System.Collections;

public class BombRock : StraightRock {

	bool stick = false;
	GameObject stickObject;
	PlayerControl stickPlayerControl;
	PlayerBehavior stickPlayerBehaviour;
	string stickPlayerName;

	GameObject thrower;


	int damage = 80;
	int AOEDamage = 10;
	float countDown = 4;
	float blastRadius = 20;

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
		float upForce = 2000;


		stickPlayerControl.allowMovement = false;

		if (stickPlayerControl.facingRight) {
			stickPlayerControl.rigidbody2D.AddForce (new Vector2 (xForce, upForce));
		} else {
			stickPlayerControl.rigidbody2D.AddForce (new Vector2 (-xForce, upForce));
		}

		if (stickPlayerBehaviour.healthBar.Health - damage > 0) {
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Ground"),LayerMask.NameToLayer(GetPlayerLayerName()),true);	
		}


		
		var hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);

		foreach (var collider in hitColliders) {
			if(collider.gameObject.tag == "Player" && !LayerMask.LayerToName(collider.gameObject.layer).Equals(GetPlayerLayerName())){
				collider.gameObject.GetComponent<PlayerBehavior>().ReduceHealth(AOEDamage);
				PlayerEvents.RecordAttack(collider.gameObject.transform.parent.gameObject,stickPlayerControl.transform.parent.gameObject,AOEDamage);
				collider.gameObject.GetComponent<PlayerBehavior>().KnockBack(this.transform.position);

			} else if(collider.gameObject.name.Contains("Rock")){
				collider.gameObject.GetComponent<ThrowableObject>().KnockBack(this.transform.position);
			} else if(collider.gameObject.name.Contains("PowerUp")){
				collider.gameObject.GetComponent<PowerUp>().KnockBack(this.transform.position);
			}
		}

		Invoke ("DoDamage", 0.25f);

	}

	public override void Throw(){
		thrower = controller.transform.parent.gameObject;
		base.Throw ();
	}

	void DoDamage(){

		PlayerEvents.RecordAttack (stickPlayerControl.transform.parent.gameObject,thrower,damage);
		PlayerEvents.RecordAttack (stickPlayerControl.transform.parent.gameObject,thrower,damage);
		stickPlayerBehaviour.ReduceHealth (damage);


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
		} else if (stickPlayerName == "PlayerFour") {
			playerLayer += "4";
		}
		return playerLayer;
	}
}
