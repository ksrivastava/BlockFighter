using UnityEngine;
using System.Collections;

public class BombRock : StraightRock {

	protected bool stick = false;
	protected GameObject stickObject;
	protected PlayerControl stickPlayerControl;
	protected PlayerBehavior stickPlayerBehaviour;
	protected string stickPlayerName;

	protected GameObject thrower;

	int damage = 80;
	int AOEDamage = 10;
	protected float countDown = 4;
	float blastRadius = 20;

	protected float countDownTimer;
	protected float secondTimer = 0f;
	private Animator anim;


	protected bool idleSinceBirth;
	protected float killTime = 10;
	protected float totalLifetime = 40;

	void Awake() {
		anim = this.GetComponent<Animator> ();
		anim.SetBool ("isExploding", false);
	}

	void CheckAndKill(){
		if (idleSinceBirth) {
			Destroy(this.gameObject);
		}
	}

	void Kill(){
		if(this.state == State.idle)
		Destroy (this.gameObject);
	}

	protected override void Start(){
		base.Start ();
		idleSinceBirth = true;
		Invoke ("CheckAndKill", killTime);
		Invoke ("Kill", totalLifetime);
	}

	public override void Update(){

		if (this.state != State.idle) {
			idleSinceBirth = false;			
		}

		if (stick) {
			foreach(var col in GetComponents<BoxCollider2D>()){
				col.enabled = false;
			}
			if(stickPlayerControl.facingRight){
				transform.position = stickObject.transform.position + new Vector3(-1,0,0);
			} else {
				transform.position = stickObject.transform.position + new Vector3(1,0,0);
			}
			Countdown ();
		}
		base.Update ();
	}

	public override void Damage (Collider2D col)
	{
		try{
			stickPlayerName = col.transform.parent.name;

			if(col.GetComponentInChildren<LeechRock>() != null || col.GetComponentInChildren<BombRock>() != null){
				return;
			}

			stick=true;
			stickObject = col.transform.gameObject;
			stickPlayerControl = stickObject.GetComponent<PlayerControl>();
			stickPlayerBehaviour =  stickObject.GetComponent<PlayerBehavior>();
			countDownTimer = countDown;

			this.transform.parent = col.transform;
			this.rigidbody2D.isKinematic = true;
			foreach (var collider in GetComponents<Collider2D>()){
				collider.enabled = false;
			}
			Invoke ("Explode",countDown);

		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}

	void Explode(){
		anim.SetBool ("isExploding", true);
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
				PlayerEvents.RecordAttack(collider.gameObject.transform.parent.gameObject,stickPlayerControl.transform.parent.gameObject,AOEDamage);
				collider.gameObject.GetComponent<PlayerBehavior>().ReduceHealth(AOEDamage);
				collider.gameObject.GetComponent<PlayerBehavior>().KnockBack(this.transform.position);

			} else if(collider.gameObject.name.Contains("Rock")){
				collider.gameObject.GetComponent<ThrowableObject>().KnockBack(this.transform.position);
			} else if(collider.gameObject.name.Contains("PowerUp")){
				collider.gameObject.GetComponent<PowerUp>().KnockBack(this.transform.position);
			}
		}

		Invoke ("DoDamage", 0.5f);
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

	public virtual void Countdown() {
		if (secondTimer > 0) {
			secondTimer -= Time.deltaTime;
		}
		
		if (secondTimer < 0) {
			secondTimer = 0;
		}
		
		if (secondTimer == 0 && countDownTimer >= 1) {
			PointsBar.DisplayNumber(this.gameObject, countDownTimer, DisplayType.Bomb);
			countDownTimer--;
			secondTimer = 1;
		}
	}
}