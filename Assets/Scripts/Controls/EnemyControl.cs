using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public float speed;
	public GameObject levelBounds;

	void Start () {
	
	}

	void Update () {
	
	}

	void FixedUpdate() {
		Vector3 bottomLeft = levelBounds.camera.ViewportToWorldPoint (Vector2.zero);
		Vector3 topRight = levelBounds.camera.ViewportToWorldPoint (new Vector2(1,1));
		Vector3 pos = transform.position;

		if (pos.x <= bottomLeft.x || pos.x + gameObject.transform.localScale.x >= topRight.x) {
			//speed *= -1;
		}

		pos.x += speed;
		transform.position = pos;

	}
}
