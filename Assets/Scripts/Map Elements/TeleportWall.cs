using UnityEngine;
using System.Collections;

public class TeleportWall : MonoBehaviour {
	private float mapLength = 70f;
	public bool leftWall;
	void OnTriggerEnter2D(Collider2D other){
		Vector2 pos = other.transform.position;
		if (leftWall) {
			pos.x += mapLength;
		} else {
			pos.x -= mapLength;
		}
		other.transform.position = pos;
	}
}
