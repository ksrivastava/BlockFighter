using UnityEngine;
using System.Collections;

public class TopLeftEntryMovement : MonoBehaviour {

	Movement m = null;
	protected Vector3 start,inPoint,depth;
	protected float dur;
	// Use this for initialization
	void Start () {

		start = Camera.main.ViewportToWorldPoint (new Vector2 (0, 1));
		inPoint = Camera.main.ViewportToWorldPoint (new Vector2 (0.3f, 0.7f));
		depth = Camera.main.ViewportToWorldPoint (new Vector2 (0.1f, 0.2f));

		//set Z for visibility
		start.z = inPoint.z = depth.z = 0;

		dur = 1.5f;

		SetMovement ();

	}
	
	// Update is called once per frame
	void Update () {
		if(m != null)
		m.Update ();
	}

	protected void SetMovement(){
		m = new Movement (this.gameObject);
		m.AddCurve (start, inPoint, dur, depth);
		m.Start ();
	}
}
