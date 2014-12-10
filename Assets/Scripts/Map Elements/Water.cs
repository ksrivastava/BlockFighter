using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player") {
			col.gameObject.GetComponent<PlayerBehavior> ().ReduceHealth (500);
			audio.Play();
		} else if(col.gameObject.name.Contains("Rock")) {
			Destroy(col.gameObject);
		}
		else if (col.gameObject.name.Contains("PowerUp")) {
			if (col.gameObject.name.Contains("StarPowerUp")) {
				Vector3 closestSpawnPoint = Vector3.zero;
				foreach (var pointObj in GameObject.FindGameObjectsWithTag("StarSpawnPoint")) {
					var point = pointObj.transform.position;
					var pointDist = Vector3.Distance(col.gameObject.transform.position, point);
					var closestSpawnPointDist = Vector3.Distance(col.gameObject.transform.position, closestSpawnPoint);
					if (closestSpawnPoint == Vector3.zero || pointDist < closestSpawnPointDist) {
						closestSpawnPoint = point;
					}
				}
				col.gameObject.transform.position = closestSpawnPoint;
			}
			else {
				Destroy(col.gameObject);
			}
		}

	}
}
