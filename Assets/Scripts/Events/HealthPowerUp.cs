using UnityEngine;
using System.Collections;

public class HealthPowerUp : PowerUp {

	float addedhealth = 50f;
	
	void Start(){

	}

	protected override void OnCollisionWithPlayerBody(GameObject player) {
		HealthBar healthBar = player.GetComponent<HealthBar>();

		float healedHeath;
		if (healthBar.MaxHealth - healthBar.Health >= addedhealth) {
			healedHeath = addedhealth;
		} else {
			healedHeath = healthBar.MaxHealth - healthBar.Health;
		}

		if(healedHeath > 0) {
			PointsBar.DisplayNumber(player, Mathf.Floor(healedHeath), DisplayType.Health);
			healthBar.AddHealth(healedHeath);
			Destroy(this.gameObject);
		}else if(healedHeath == 0){
			Destroy(this.gameObject);
		}
	}
}
