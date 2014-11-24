using UnityEngine;
using System.Collections;

public class LeechPowerUpEvent : PowerUpEvent {

	protected void Start() {
		EventController.DisplayMessage ("Poison Rock!");
		path = "ThrowableObjects/LeechRock";
		eventType = RunnableEventType.BombPowerUpEvent;
		base.Start ();
	}
}
