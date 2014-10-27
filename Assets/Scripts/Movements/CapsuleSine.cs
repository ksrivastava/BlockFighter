using UnityEngine;
using System.Collections;

public class CapsuleSine : MonoBehaviour {

	Movement m;
	public GameObject marker;
	// Use this for initialization
	void Start () {
		m = new Movement (this.gameObject);
		Vector3 startPos = this.transform.position;
		m.AddSine (startPos, startPos + new Vector3 (-60, 0, 0), 5, 2, 1);
		m.ChainSine (startPos, 5, 2, 1);
		m.SetRepeat ();
		m.setMarker (marker);
		m.ToggleTrail ();
		m.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		m.Update ();
	}
}
