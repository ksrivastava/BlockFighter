using UnityEngine;
using System.Collections;

public class LeechPowerUpEvent : PowerUpEvent {

	protected void Start() {
		path = "ThrowableObjects/LeechRock";
		eventType = EventType.BombPowerUpEvent;
		base.Start ();
	}
}
