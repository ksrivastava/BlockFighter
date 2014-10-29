using UnityEngine;
using System.Collections;

public class LittleRock : ThrowableObject {

	public State startingState = State.idle;

	void Start(){
		this.state = startingState;
	}

	// little rocks cause 5 damage?
	public override void Damage (Collider2D col)
	{
		try{
			col.gameObject.GetComponent<PlayerBehavior>().ReduceHealth(5);
		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}
}
