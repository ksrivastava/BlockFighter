using UnityEngine;
using System.Collections;

public class EventRunner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EventController.QueueEvent (EventType.Blimp);
		StartCoroutine(EventController.NextEvent(1));
	}
	
	// Update is called once per frame
	void Update () {
		if (EventController.CanRunNextEvent ()) {
			StartCoroutine(EventController.NextEvent());
		}
	}
}
