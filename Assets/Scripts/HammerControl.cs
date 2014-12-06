using UnityEngine;
using System.Collections;
using InControl;

public class HammerControl : MonoBehaviour {

	public bool isBigHammer;

	public Sprite hammerBody;
	public Sprite bigHammerBody;

	private float aoeXRange = 7f;
	private float aoeYRange = 1f;
	private float aoeForce = 500f;

	private Vector3 origScale;

	[HideInInspector]
	public bool isHitting = false;
	bool isJabbing = false;
	bool doAoe = false;
	float speed = 700f;

	PlayerControl controller;
	Transform player;
	Collider2D collider;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find (transform.parent.name + "/Body").GetComponent<PlayerControl> ();
		player = GameObject.Find (transform.parent.name + "/Body").transform;
		collider = GameObject.Find (transform.parent.name + "/Hammer/Body").collider2D;
		origScale = GameObject.Find (transform.parent.name + "/Hammer/Body").transform.localScale;
	}

	float duration = 0.1f;
	float deltaTime = 0f;
	[HideInInspector]
	public bool attackComplete = false;
	int direction;

	public void Hit(){
		if (!isHitting) {
			isHitting = true;
			controller.allowMovement = false;
		}
	}

	// Update is called once per frame
	void Update () {

		var pos = player.position;
		direction = (controller.facingRight) ? 1 : -1;
		pos.x += (player.renderer.bounds.size.x/2) * direction;
		pos.y += 0.1f;
		pos.z = -1;
		transform.position = pos;

		GameObject body = GameObject.Find (transform.parent.name + "/Hammer/Body");
		if (isBigHammer) {
			Vector3 scale = body.transform.localScale;
			scale.x = 10.0f;
			scale.y = 10.0f;
			body.transform.localScale = scale;
		} else {
			body.transform.localScale = origScale;
		}

		if (isHitting) {
			if (attackComplete) {
				if (deltaTime >= duration) {
					if ((isJabbing && StopJab()) ||
					    (!isJabbing && SwingUp())) {
						isHitting = false;
						isJabbing = false;
						attackComplete = false;
						collider.enabled = true;
						controller.allowMovement = true;
						deltaTime = 0f;
					}
				}
				else {
					deltaTime += Time.deltaTime;
				}
				if (doAoe) {
					AOEDamage ();
					doAoe = false;
				}
			}
			else {
				if (isJabbing) {
					attackComplete = StartJab();
				}
				else {
					attackComplete = SwingDown();
				}
				if (attackComplete && isBigHammer) {
					doAoe = true;
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

	Vector2 angleOne = new Vector2 (0, 250); // 270 - 20
	Vector2 angleTwo = new Vector2 (360, 110); // 90 + 20

	bool SwingDown() {
		int dir = (Physics2D.gravity.y > 0) ? -direction : direction;

		if (Physics2D.gravity.y > 0) {
			angleOne.Set(180, 70);
			angleTwo.Set(180, 290);
		}
		else {
			angleOne.Set(0, 250);
			angleTwo.Set(360, 110);
		}

		transform.Rotate(0, 0, -speed * Time.deltaTime * dir);
		float z = transform.rotation.eulerAngles.z;
		if (
			((dir == 1) && z <= angleOne.y) || // facing right, and is at <= (290)
			((dir == -1) && z >= angleTwo.y) // facing left, and is at >= 110
			) {
			return true;
		}
		return false;
	}

	bool SwingUp() {
		int dir = (Physics2D.gravity.y > 0) ? -direction : direction;

		if (Physics2D.gravity.y > 0) {
			angleOne.Set(180, 290);
			angleTwo.Set(180, 70);
		}
		else {
			angleOne.Set(0, 250);
			angleTwo.Set(360, 110);
		}

		transform.Rotate(0, 0, speed * Time.deltaTime * dir);
		float z = transform.rotation.eulerAngles.z;

		if (
			((dir == 1) && z >= angleOne.x && z <= angleOne.y) ||
			((dir == -1) && z <= angleTwo.x && z >= angleTwo.y)
			) {
			
			var rot = transform.rotation;
			rot.z = angleOne.x;
			transform.rotation = rot;
			
			return true;
		}
		return false;
	}

	public void AOEDamage() {
		float hammerX = transform.position.x;
		GameObject[] pl = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < pl.Length; ++i) {
			float pX = pl[i].transform.position.x;
			float pY = pl[i].transform.position.y;
			PlayerControl pC = pl[i].GetComponent<PlayerControl>();
			if (pC != controller && Mathf.Abs(hammerX - pX) < aoeXRange && Mathf.Abs(pY - player.position.y) < aoeYRange) {
				if (pC.IsGrounded()) {
					pC.rigidbody2D.AddForce (new Vector2(0f, aoeForce));
					pl[i].GetComponent<PlayerBehavior>().ReduceHealth(20);
				}
			}
		}
	}

	public void UpgradeHammerTime(float time) {
		isBigHammer = true;
		GameObject body = GameObject.Find (transform.parent.name + "/Hammer/Body");
		body.GetComponent<SpriteRenderer> ().sprite = bigHammerBody;
		Vector3 scale = body.transform.localScale;
		scale.x = 10.0f;
		scale.y = 10.0f;
		body.transform.localScale = scale;
		StartCoroutine (StopPowerUpAfterTime (time));
	}

	private IEnumerator StopPowerUpAfterTime(float time) {
		yield return new WaitForSeconds (time);
		GameObject body = GameObject.Find (transform.parent.name + "/Hammer/Body");
		body.GetComponent<SpriteRenderer> ().sprite = hammerBody;
		body.transform.localScale = origScale;
		isBigHammer = false;
	}
}
