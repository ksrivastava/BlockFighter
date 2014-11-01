using UnityEngine;
using System.Collections;

public class LittleRockShower : StraightRockShower {

	IEnumerator Start(){
		this.path = "ThrowableObjects/LittleRock";
		EventController.DisplayMessage ("LittleRocks!!!",2, new Vector2(0.5f,0.9f));
		return base.Start ();
	}

	public override void End(){
		EventController.EventEnd (EventType.LittleRockShower, EventType.PointLights, 3);
//		EventController.EventEnd (EventType.LittleRockShower, EventType.MapSpin, 3);
	}
}
