using UnityEngine;
using System.Collections;

public class StarPowerUp : PowerUp {

	protected override void OnCollisionWithPlayerBody(GameObject player) {
		PointsBar.AddStars (player);
		Destroy(this.gameObject);
	}
}
