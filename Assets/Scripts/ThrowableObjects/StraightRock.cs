using UnityEngine;
using System.Collections;

public class StraightRock : ThrowableObject {

	public State startingState = State.idle;

	void Start(){
		this.throwForce = 0;
		this.xMult = 2000f;
		this.state = startingState;
		this.damageVal = 10;
	}
	
	// little rocks cause 5 damage?
	public override void Damage (Collider2D col)
	{
		try{
			if (col.gameObject.GetComponent<PlayerBehavior>()) {
				col.gameObject.GetComponent<PlayerBehavior>().ReduceHealth(damageVal);
				col.gameObject.GetComponent<PlayerBehavior>().KnockBack(this.transform.position);
			} else if (col.gameObject.GetComponent<EnemyBehavior>()) {
				col.gameObject.GetComponent<EnemyBehavior>().ReduceHealth(damageVal);
			}
		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}
}
