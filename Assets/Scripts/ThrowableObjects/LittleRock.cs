using UnityEngine;
using System.Collections;

public class LittleRock : ThrowableObject {

	// little rocks cause 5 damage?
	public override void Damage (Collider2D col)
	{
		try{
			col.gameObject.GetComponent<PlayerBehavior>().Health -= 5;
		} catch(UnityException ex){
			print ("Player doesn't exist");
		}
	}
}
