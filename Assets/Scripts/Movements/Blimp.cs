using UnityEngine;
using System.Collections;

public class Blimp : MonoBehaviour, IEvent {

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

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "ThrowableObject") {
			ThrowableObject obj = other.gameObject.GetComponent<ThrowableObject>();
			if(obj.player != null){
				Destroy(this.gameObject);
			}
		}
	}

	public void Begin(){

	}

	public void End(){
		EventController.EventEnd (EventType.Blimp,  EventType.Bomb, 1);
	}

	public void OnDestroy(){
		End ();
	}
}
