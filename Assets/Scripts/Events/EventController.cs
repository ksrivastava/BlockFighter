using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController : MonoBehaviour {		

	private static GameObject eventRunner;
	private static float timeScale = 1.25f;

	public static Queue<EventHelper> verticalSliceEventQueue = new Queue<EventHelper>();

	// Use this for initialization
	void Start () {

		verticalSliceEventQueue.Enqueue (new EventHelper(EventType.LittleRockShower, 1));
		verticalSliceEventQueue.Enqueue (new EventHelper(EventType.EnemyEvent, 1));
		verticalSliceEventQueue.Enqueue (new EventHelper(EventType.PointLights, 1));
		verticalSliceEventQueue.Enqueue (new EventHelper(EventType.Blimp, 1));

		eventQueue.Add (verticalSliceEventQueue.Dequeue());
		StartCoroutine(NextEvent());
		eventRunner = GameObject.Find ("EventRunner");

		//TODO: DELETE THIS LINE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		InvokeRepeating ("TestCheckEvents",0, 1);
	}

	//TODO: DELETE THIS Function
	void TestCheckEvents(){
		PlayerEvents.CheckPlayerEvents ();
	}
	
	// Update is called once per frame
	void Update () {
		if (CanRunNextEvent ()) {
			StartCoroutine(NextEvent());
		}
	}
	
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
			//Debug.Log("Waiting " + next.delay);
			yield return new WaitForSeconds (next.delay);
		}

		//Debug.Log("Running " + next.type);
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
	public static void DisplayMessage(string message,float seconds,Vector2 pos, float startTime=0, int fontsize=15){
		eventRunner.GetComponent<Message> ().DisplayMessage (message, seconds, pos, startTime*timeScale,fontsize);
	}
	

	// The controller has been notified here that an event has ended. It has also suggested a
	// next event to run and a delay time before running it.
	public static void EventEnd(EventType running, EventType nextState, float delay = 0){

		var nextEvent = verticalSliceEventQueue.Dequeue ();
		eventQueue.Add (nextEvent);
//		ExecuteEventTransition (nextState, delay);
		eventLock = false;
	}

	public static void ExecuteEventTransition(EventType nextState, float delay){

		//TODO: encode state machine
		// if(currentEvent == blablabla)
		QueueEvent (nextState, delay);
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