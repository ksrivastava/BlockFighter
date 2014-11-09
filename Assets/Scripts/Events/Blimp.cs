using UnityEngine;
using System.Collections;

public class Blimp : MonoBehaviour, IEvent {

	Movement m;
	public GameObject marker;
	// Use this for initialization
	void Start () {
		m = new Movement (this.gameObject);
		var startPos = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0.9f,0f));
		startPos.z = 0f;
		var endPos = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0.9f, 0f));
		endPos.z = 0f;
		m.AddSine ( startPos, endPos, 5, 2, 1);
		m.ChainSine (startPos, 5, 2, 1);
		m.SetRepeat ();
		m.setMarker (marker);
		m.Start ();
	}

	void FixedUpdate(){
		m.ToggleTrail ();
	}
	
	// Update is called once per frame
	void Update () {
		m.Update ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "ThrowableObject") {
			ThrowableObject obj = other.gameObject.GetComponent<ThrowableObject>();
			if(obj.hammer != null){
				Destroy(this.gameObject);
			}
		}
	}

	public void Begin(){

	}

	public void End(){
		EventController.EventEnd (EventType.Blimp,  EventType.Idle, 1);
	}

	public void OnDestroy(){
		End ();
	}
}
