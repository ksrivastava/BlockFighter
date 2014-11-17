﻿using UnityEngine;
using System.Collections;

public class HealthPowerUp : PowerUp {

	public float addedhealth = 30f;

	protected override void OnCollisionWithPlayerBody(GameObject player) {
		HealthBar healthBar = player.GetComponent<HealthBar>();
		if (healthBar.Health < healthBar.MaxHealth) {
			healthBar.AddHealth(addedhealth);
			Destroy(this.gameObject);
		}
	}
}
