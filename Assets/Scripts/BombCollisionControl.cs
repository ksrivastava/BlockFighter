using UnityEngine;
using System.Collections;

public class BombCollisionControl : MonoBehaviour {

	public float bombPushStrength;
	private bool falling = true;

	void FixedUpdate() {
		if(falling) {
			Vector2 bombPos = transform.position;
			bombPos.y -= 30f * Time.deltaTime;
			transform.position = bombPos;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.name == "Ground") {
			falling = false;
			Vector2 bombPos = transform.position;
			bombPos.y = col.transform.position.y + col.renderer.bounds.size.y/2 + renderer.bounds.size.y/2;
			transform.position = bombPos;
		} else if (col.tag == "Hammer" && col.name == "Body") {
			HammerControl hammer = (HammerControl)col.gameObject.transform.parent.GetComponent("HammerControl");
			PlayerControl controller = (PlayerControl)col.gameObject.transform.parent.transform.parent.GetChild(0).GetComponent("PlayerControl");
			if(hammer.isHitting) {
				int direction = (controller.facingRight) ? 1 : -1;
				Vector2 bombPos = transform.position;
				bombPos.x += direction * bombPushStrength * Time.deltaTime;
				transform.position = bombPos;
			}
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		OnTriggerEnter2D (col);
	}
}
