using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {


	float height=20;
	float durationUp = 5;
	float durationDown = 1;

	Movement m = null; 

	void Start(){
		m = new Movement(this.gameObject);
		Vector3 start = transform.position;
		Vector3 end = transform.position;
		end.y += height;
		m.AddLine (start, end, durationUp);
		m.AddLine (end, start, durationDown);
		m.SetRepeat ();
		m.Start ();
	}
	
	void Update () {
		m.Update ();
	}
}
