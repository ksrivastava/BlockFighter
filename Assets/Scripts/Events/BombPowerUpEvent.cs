using UnityEngine;
using System.Collections;

public class BombPowerUpEvent : PowerUpEvent {

	protected void Start() {
		EventController.DisplayMessage ("Bomb!");
		path = "ThrowableObjects/BombRock";
		eventType = EventType.BombPowerUpEvent;
		base.Start ();
	}
}
