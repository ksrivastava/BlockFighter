using UnityEngine;
using System.Collections;

public class StraightRock : ThrowableObject {

	public State startingState = State.idle;

	void Start(){
		this.throwForce = 0;
		this.xMult = 2000f;
		this.state = startingState;
		this.damageVal = 10;


		//TODO: DELETE THIS
		PlayerEvents.CheckPlayerGangedUpOn (PlayerEvents.GetPlayerStats("PlayerOne"));
	}
	
	// little rocks cause 5 damage?
	public override void Damage (Collider2D col)
	{
		try{
			col.gameObject.GetComponent<PlayerBehavior>().ReduceHealth(damageVal);
		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}
}
