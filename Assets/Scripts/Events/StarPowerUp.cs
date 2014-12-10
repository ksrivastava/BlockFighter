using UnityEngine;
using System.Collections;

public class StarPowerUp : PowerUp {

	public AudioClip starClip;

	protected override void OnCollisionWithPlayerBody(GameObject player) {
		PointsBar.AddStars (player);
		MusicPlayer.PlaySound (starClip, 2f);
		Destroy(this.gameObject);
	}

	protected override void Awake(){

	}
}
