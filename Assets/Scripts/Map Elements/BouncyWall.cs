using UnityEngine;
using System.Collections;

public class BouncyWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if( LayerMask.LayerToName(other.gameObject.layer).Contains("Player")){
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Ground"), other.gameObject.layer, false);
		}
	}
}
