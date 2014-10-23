using UnityEngine;
using System.Collections;

public class HammerControl : MonoBehaviour {

	bool isHitting = false;
	float speed = 400f;

	PlayerControl controller;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find (transform.parent.name + "/Body").GetComponent<PlayerControl> ();
	}

	float duration = 0.2f;
	float deltaTime = 0f;
	[HideInInspector]
	public bool isGoingUp = false;
	int direction = 1;
	
	// Update is called once per frame
	void Update () {

		Transform obj = GameObject.Find (transform.parent.name + "/Body").transform;
		var pos = obj.position;
		direction = (controller.facingRight) ? 1 : -1;
//		pos.y += obj.renderer.bounds.size.y/2;
		pos.x += (obj.renderer.bounds.size.x/2 + 0.2f) * direction;
		transform.position = pos;

		string fireInput = controller.isSecondPlayer ? "Fire2" : "Fire1";

		if (Input.GetAxis(fireInput) > 0 && !isHitting) {
			isHitting = true;
			controller.enabled = false;
		}
		
		if (isHitting) {

			if (isGoingUp) {
				if (deltaTime >= duration) {
					transform.Rotate(0, 0, speed * Time.deltaTime * direction);
					float z = transform.rotation.eulerAngles.z;
					if (
						((direction == 1) && z >= 0 && z < 270) ||
						((direction == -1) && z <= 360 && z > 90)
						) {

							var rot = transform.rotation;
							rot.z = 0;
							transform.rotation = rot;

							isHitting = false;
							isGoingUp = false;
							controller.enabled = true;

							deltaTime = 0f;
					}
				}
				else {
					deltaTime += Time.deltaTime;
				}

			}
			else {
				transform.Rotate(0, 0, - speed * Time.deltaTime * direction);
				float z = transform.rotation.eulerAngles.z;
				if (
					((direction == 1) && z <= 270 - 20) ||
					((direction == -1) && z >= 90 + 20)
				) {
					isGoingUp = true;
				}
			}
		}
	}
}
