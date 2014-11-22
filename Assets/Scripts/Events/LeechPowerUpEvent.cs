using UnityEngine;
using System.Collections;

public class LeechPowerUpEvent : PowerUpEvent {

	protected void Start() {
		EventController.DisplayMessage ("Leech!");
		path = "ThrowableObjects/LeechRock";
		eventType = RunnableEventType.BombPowerUpEvent;
		base.Start ();
	}
}
