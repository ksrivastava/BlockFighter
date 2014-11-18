using UnityEngine;
using System.Collections;

public class BombPowerUpEvent : PowerUpEvent {

	protected void Start() {
		path = "ThrowableObjects/BombRock";
		eventType = EventType.BombPowerUpEvent;
		base.Start ();
	}
}
