using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

	public float bottom;
	public float top;
	public float speed;
	
	void Update () {
		if(transform.position.y < bottom) {
			speed *= -1;
		} else if (transform.position.y > top) {
			speed *= -1;
		}

		Vector2 pos = transform.position;
		pos.y += speed * Time.deltaTime;
		transform.position = pos;
	}
}
