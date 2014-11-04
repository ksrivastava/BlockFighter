using UnityEngine;
using System.Collections;


//TODO: do we actually need this? I could do this in EventController.
public class EventRunner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("TestStuff", 30);
		EventController.QueueEvent (EventType.Idle);
		StartCoroutine(EventController.NextEvent());
	}
	
	// Update is called once per frame
	void Update () {
		if (EventController.CanRunNextEvent ()) {
			StartCoroutine(EventController.NextEvent());
		}
	}

	void TestStuff(){
		PlayerEvents.CheckPlayerGangedUpOn (PlayerEvents.GetPlayerStats ("PlayerOne"));
	}
}
