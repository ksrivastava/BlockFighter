using UnityEngine;
using System.Collections;

public class BigHammerEvent : PowerUpEvent {
	
	protected void Start() {
		EventController.DisplayMessage ("Big Hammer!");
		path = "PowerUp/HammerPowerUp";
		eventType = RunnableEventType.BigHammerPowerUpEvent;
		base.Start ();
	}
}
