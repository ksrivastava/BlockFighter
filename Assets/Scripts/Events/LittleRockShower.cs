using UnityEngine;
using System.Collections;

public class LittleRockShower : StraightRockShower {

	IEnumerator Start(){
		this.path = "ThrowableObjects/LittleRock";
		EventController.DisplayMessage ("LittleRocks!!!");
		return base.Start ();
	}

	public override void End(){
		EventController.EventEnd (RunnableEventType.LittleRockShower, RunnableEventType.Idle, 10);
	}
}
