using UnityEngine;
using System.Collections;

public class BombCollisionControl : MonoBehaviour {

	public float bombPushStrength;
	private bool falling = true;
	public float health = 10;


	private bool vulnerable = true;
	private float vulnTime = 1f;


	void FixedUpdate() {
		if(falling) {
			Vector2 bombPos = transform.position;
			bombPos.y -= 30f * Time.deltaTime;
			transform.position = bombPos;
		}
	}

	void Update(){
		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			print ("Collided with ground");
			falling = false;
			Vector2 bombPos = transform.position;
			bombPos.y = col.transform.position.y + col.renderer.bounds.size.y/2 + renderer.bounds.size.y/2;
			transform.position = bombPos;
		} else if (col.tag == "Hammer" /*&& col.name == "Body"*/) {

			HammerControl hammer = (HammerControl)col.gameObject.transform.parent.GetComponent("HammerControl");
			PlayerControl controller = (PlayerControl)col.gameObject.transform.parent.transform.parent.GetChild(0).GetComponent("PlayerControl");
			if(hammer.isHitting && vulnerable) {
				print ("Collided with hammer!");
				int direction = (controller.facingRight) ? 1 : -1;
				Vector2 bombPos = transform.position;
				bombPos.x += direction * bombPushStrength * Time.deltaTime;
				transform.position = bombPos;
				this.health--;
				vulnerable = false;
				Invoke("setVulnerable",vulnTime);
			}
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		OnTriggerEnter2D (col);
	}

	void setVulnerable(){
		vulnerable = true;
	}
}
