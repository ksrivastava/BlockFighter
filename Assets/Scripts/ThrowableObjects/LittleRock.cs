using UnityEngine;
using System.Collections;

public class LittleRock : ThrowableObject {

	public State startingState = State.idle;

	void Start(){
		this.state = startingState;
		this.damageVal = 5;
	}

	// little rocks cause 5 damage?
	public override void Damage (Collider2D col)
	{
		try{
			if (col.gameObject.GetComponent<PlayerBehavior>()) {
				col.gameObject.GetComponent<PlayerBehavior>().ReduceHealth(damageVal);
			} else if (col.gameObject.GetComponent<EnemyBehavior>()) {
				col.gameObject.GetComponent<EnemyBehavior>().ReduceHealth(damageVal);
			}
		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}
}
