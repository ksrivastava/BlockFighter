using UnityEngine;
using System.Collections;

public class HammerControl : MonoBehaviour {

	[HideInInspector]
	public bool isHitting = false;
	bool isJabbing = false;
	float speed = 400f;

	PlayerControl controller;
	Transform player;
	HammerCollision collision;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find (transform.parent.name + "/Body").GetComponent<PlayerControl> ();
		player = GameObject.Find (transform.parent.name + "/Body").transform;
		collision = GameObject.Find (transform.parent.name + "/Hammer/Body").GetComponent<HammerCollision> ();
	}

	float duration = 0.2f;
	float deltaTime = 0f;
	[HideInInspector]
	public bool attackComplete = false;
	int direction = 1;
	
	// Update is called once per frame
	void Update () {

		var pos = player.position;
		direction = (controller.facingRight) ? 1 : -1;
		pos.x += (player.renderer.bounds.size.x/2 + 0.2f) * direction;
		pos.z = -1;
		transform.position = pos;

		string fireInput = controller.isSecondPlayer ? "Fire2" : "Fire1";
		float v = controller.isSecondPlayer? Input.GetAxis("Vertical2") : Input.GetAxis("Vertical");

		if (Input.GetAxis(fireInput) > 0 && !isHitting) {
			isHitting = true;
			controller.enabled = false;

			if (v < 0) {
				isJabbing = true;
			}
		}
		
		if (isHitting) {

			if (attackComplete) {
				if (deltaTime >= duration) {
					if ((isJabbing && StopJab()) ||
					    (!isJabbing && SwingUp())) {
						isHitting = false;
						isJabbing = false;
						attackComplete = false;
						collision.Enable();
						controller.enabled = true;
						deltaTime = 0f;
					}
				}
				else {
					deltaTime += Time.deltaTime;
				}

			}
			else {
				if (isJabbing) {
					attackComplete = StartJab();
				}
				else {
					attackComplete = SwingDown();
				}
			}
		}
	}

	bool StopJab() {
		return SwingUp ();
	}

	bool StartJab() {
		return SwingDown ();
	}

	bool SwingDown() {
		transform.Rotate(0, 0, - speed * Time.deltaTime * direction);
		float z = transform.rotation.eulerAngles.z;
		if (
			((direction == 1) && z <= 270 - 20) ||
			((direction == -1) && z >= 90 + 20)
			) {
			return true;
		}

		return false;
	}

	bool SwingUp() {
		transform.Rotate(0, 0, speed * Time.deltaTime * direction);
		float z = transform.rotation.eulerAngles.z;
		if (
			((direction == 1) && z >= 0 && z < 270) ||
			((direction == -1) && z <= 360 && z > 90)
			) {
			
			var rot = transform.rotation;
			rot.z = 0;
			transform.rotation = rot;

			return true;
		}

		return false;
	}
}
