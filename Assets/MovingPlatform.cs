using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	Movement m;
	public Vector2 displacement = new Vector2(10,0);
	public float duration = 5;
	public bool moveRightFirst = true;

	// Use this for initialization
	void Start () {
		m = new Movement (this.gameObject);
		if(moveRightFirst){
			m.AddLine(this.transform.position,(Vector2)this.transform.position + displacement, duration);
		} else {
			m.AddLine(this.transform.position,(Vector2)this.transform.position - displacement, duration);
		}

		m.ChainLine (this.transform.position, duration);
		m.SetRepeat ();
		m.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		m.Update ();
		var pos = this.transform.position;
		pos.z = -9;
		this.transform.position = pos;
	}
}
