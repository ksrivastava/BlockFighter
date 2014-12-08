using UnityEngine;
using System.Collections;

public class BigHammerPowerup : PowerUp {

	private HammerControl hc;

	protected override void OnCollisionWithPlayerBody(GameObject player) {
		hc = GameObject.Find (player.transform.parent.name + "/Hammer").GetComponent<HammerControl> ();
		//if (!hc.isBigHammer) {
		hc.UpgradeHammerTime(8.0f);
		Destroy (this.gameObject);
		//}
	}
}
