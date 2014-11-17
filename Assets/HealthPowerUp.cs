using UnityEngine;
using System.Collections;

public class HealthPowerUp : MonoBehaviour {

	public float addedhealth = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "Player") {
			HealthBar healthBar = col.gameObject.GetComponent<HealthBar>();
			if (healthBar.Health < healthBar.MaxHealth) {
				healthBar.AddHealth(addedhealth);
				Destroy(this.gameObject);
			}
		}
	}
}
