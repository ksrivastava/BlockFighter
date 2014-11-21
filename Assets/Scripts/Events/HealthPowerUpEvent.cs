using UnityEngine;
using System.Collections;

public class HealthPowerUpEvent : PowerUpEvent {

	protected void Start() {
		EventController.DisplayMessage ("Health!");
		path = "PowerUp/HealthPowerUp";
		eventType = EventType.HealthPowerUpEvent;
		base.Start ();
	}
}
