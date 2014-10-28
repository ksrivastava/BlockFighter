using UnityEngine;
using System.Collections;

public class StraightRock : ThrowableObject {

	public State startingState = State.idle;

	void Start(){
		this.throwForce = 0;
		this.xMult = 1000f;
		this.state = startingState;
	}
	
	// little rocks cause 5 damage?
	public override void Damage (Collider2D col)
	{
		try{
			col.gameObject.GetComponent<PlayerBehavior>().Health -= 10;
		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}
}
