﻿using UnityEngine;
using System.Collections;

public class LeechRock : BombRock {

	void Awake(){
		this.countDown = 10;

	}

	public override void Damage (Collider2D col){
		try{
			stickPlayerName = col.transform.parent.name;

			if(col.GetComponentInChildren<LeechRock>() != null){
				return;
			}

			stick=true;
			stickObject = col.transform.gameObject;
			stickPlayerControl = stickObject.GetComponent<PlayerControl>();
			stickPlayerBehaviour =  stickObject.GetComponent<PlayerBehavior>();
			countDownTimer = countDown;

			this.transform.parent = stickPlayerControl.gameObject.transform;
			this.rigidbody2D.isKinematic = true;
			foreach (var collider in GetComponents<Collider2D>()){
				collider.enabled = false;
			}
			Invoke ("Detach",countDown*2);
			
		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}

	void Detach(){
		Destroy (this.gameObject);
	}

	public override void Countdown() {
		if (secondTimer > 0) {
			secondTimer -= Time.deltaTime;
		}
		
		if (secondTimer < 0) {
			secondTimer = 0;
		}
		
		if (secondTimer == 0 && countDownTimer >= 1) {
			//PointsBar.DisplayNumber(this.gameObject, countDownTimer, DisplayType.Bomb);
			stickPlayerBehaviour.ReduceHealth(5);
			countDownTimer--;
			secondTimer = 2;
		}
	}
}
