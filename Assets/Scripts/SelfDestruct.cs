using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	public float duration = 0.2f;
	public float deltaTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (deltaTime > duration) {
			Destroy(this.gameObject);
		}
		else {
			deltaTime += Time.deltaTime;
		}
	}
}
