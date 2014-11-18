﻿using UnityEngine;
using System.Collections;

public class HealthPowerUp : PowerUp {

	float addedhealth = 50f;

	protected override void OnCollisionWithPlayerBody(GameObject player) {
		HealthBar healthBar = player.GetComponent<HealthBar>();
		if (healthBar.Health < healthBar.MaxHealth) {
			PlayerControl c = player.GetComponentInChildren<PlayerControl>();
			PointsBar.DisplayNumber(c, addedhealth, DisplayType.Health);
			healthBar.AddHealth(addedhealth);
			Destroy(this.gameObject);
		}
	}
}
