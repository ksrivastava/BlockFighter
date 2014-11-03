using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventController {		

	static EventType currentEvent;

	public static List<EventHelper> eventQueue = new List<EventHelper>();

	private static bool eventLock = false;

	public static IEnumerator  NextEvent(){

		eventLock = true;
		while (eventQueue.Count == 0) {
			Debug.Log("No more events to run!");
			yield return null; 
		}

	
	   	// pull event off Queue
		// instantiate gameobject
		var next = eventQueue [0];
		eventQueue.RemoveAt (0);

		
		if (next.delay != 0) {
			Debug.Log("Waiting " + next.delay);
			yield return new WaitForSeconds (next.delay);
		}

		Debug.Log("Running " + next.type);
		currentEvent = next.type;

		if (next.type != EventType.Idle) {
			GameObject obj = Object.Instantiate (Resources.Load ("Events/"+next.type.ToString ())) as GameObject;
			IEvent evt = (IEvent) obj.GetComponent(typeof(IEvent));
			evt.Begin();
		}

		yield return null;
	}
	
	public static void QueueEvent(EventType e,float delay=0){
		eventQueue.Add (new EventHelper(e,delay));
	}

	// pos is in viewport coordinates ( bottom-left is 0,0 and top-Right is 1,1)
	public static void DisplayMessage(string message,float seconds,Vector2 pos){
		GameObject obj = Object.Instantiate (Resources.Load ("Message")) as GameObject;
		GUIText guiText = obj.GetComponent<GUIText> ();
		guiText.text = message;
		guiText.transform.position = pos;
		MessageLife m = obj.GetComponent<MessageLife> ();
		m.Kill (seconds);
	}

	// The controller has been notified here that an event has ended. It has also suggested a
	// next event to run and a delay time before running it.
	public static void EventEnd(EventType running, EventType nextState, float delay = 0){
	
		//check with PlayerEvents whether anything player related has happened.
	
		QueueEvent (nextState,delay);
		eventLock = false;
	}

	public static bool CanRunNextEvent(){
		return !eventLock;
	}

	public class EventHelper{
		public EventType type;
		public float delay;

		public EventHelper(EventType t,float d){
			type=t;
			delay=d;
		}
	}

}