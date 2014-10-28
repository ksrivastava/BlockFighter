using UnityEngine;
using System.Collections;

public class TopRightEntryMovement : TopLeftEntryMovement {

	// Use this for initialization
	void Start () {
		
		start = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
		inPoint = Camera.main.ViewportToWorldPoint (new Vector2 (0.7f, 0.7f));
		depth = Camera.main.ViewportToWorldPoint (new Vector2 (0.9f, 0.2f));
		
		//set Z for visibility
		start.z = inPoint.z = depth.z = 0;
		
		dur = 1.5f;

		SetMovement ();
	}
}
