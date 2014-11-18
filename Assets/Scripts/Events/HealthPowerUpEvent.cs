using UnityEngine;
using System.Collections;

public class HealthPowerUpEvent : PowerUpEvent {

	protected void Start() {
		path = "PowerUp/HealthPowerUp";
		eventType = EventType.HealthPowerUpEvent;
		base.Start ();
	}
}
