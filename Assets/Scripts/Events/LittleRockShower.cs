using UnityEngine;
using System.Collections;

public class LittleRockShower : StraightRockShower {

	IEnumerator Start(){
		this.path = "ThrowableObjects/LittleRock";
		return base.Start ();
	}

	public override void End(){
		//EventController.EventEnd (EventType.LittleRockShower, EventType.StraightRockShower, 3);
//		EventController.EventEnd (EventType.LittleRockShower, EventType.MapSpin, 3);
	}
}
