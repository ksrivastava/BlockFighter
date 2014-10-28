using UnityEngine;
using System.Collections;

public class EventRunner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EventController.QueueEvent (EventType.LittleRockShower);
		StartCoroutine(EventController.NextEvent());
	}
	
	// Update is called once per frame
	void Update () {
		if (EventController.CanRunNextEvent ()) {
			StartCoroutine(EventController.NextEvent());
		}
	}
}
