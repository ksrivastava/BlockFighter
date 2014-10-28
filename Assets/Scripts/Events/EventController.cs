using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventController {		

	static EventType currentEvent;

	public static List<EventType> eventQueue = new List<EventType>();
	public static EventType previousState = EventType.Idle;

	private static bool eventLock = false;

	public static IEnumerator  NextEvent( float delay=0){

		eventLock = true;
		if (delay != 0) {
			Debug.Log("Waiting " + delay);
			yield return new WaitForSeconds (delay);
		}
		if (eventQueue.Count == 0) {
			Debug.Log("No more events to run!");
			yield return null; 
		}

	
	   	// pull event off Queue
		// instantiate gameobject
		var nextEventType = eventQueue [0];
		eventQueue.RemoveAt (0);
		
		Debug.Log("Running " + nextEventType);

		if (nextEventType != EventType.Idle) {
			GameObject obj = Object.Instantiate (Resources.Load (nextEventType.ToString ())) as GameObject;
			IEvent evt = (IEvent) obj.GetComponent(typeof(IEvent));
			evt.Begin();
		}

		yield return null;
	}
	
	public static void QueueEvent(EventType e){
		eventQueue.Add (e);
	}

	public static void EventEnd(EventType running, EventType nextState, float delay = 0){
		QueueEvent (nextState);
		eventLock = false;
	}

	public static bool CanRunNextEvent(){
		return !eventLock;
	}

}