using UnityEngine;
using System.Collections;

public class PointLightHealthBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (LayerMask.LayerToName (col.gameObject.layer).Contains ("Player")) {
			// turn on the health bar
			getTopParent(col.gameObject).GetComponentInChildren<PlayerControl>().healthBar.showBars = true;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (LayerMask.LayerToName (col.gameObject.layer).Contains ("Player")) {
			// turn on the health bar
			getTopParent(col.gameObject).GetComponentInChildren<PlayerControl>().healthBar.showBars = false;
		}
	}
		
	GameObject getTopParent(GameObject input){
		Transform t = input.transform;
		Transform p = input.transform.parent;
		
		while (p != null) {
			t = p;
			p = p.transform.parent;
		}
		
		return t.gameObject;
	}
}
