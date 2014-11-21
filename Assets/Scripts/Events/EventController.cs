using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController : MonoBehaviour {		

	private static GameObject eventRunner;


	// Use this for initialization
	void Start () {

		//QueueEvent (EventType.StraightRockShower);
		//StartCoroutine(NextEvent());
		eventRunner = GameObject.Find ("EventRunner");

		//TODO: DELETE THIS LINE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		InvokeRepeating ("TestFormTeam",0, 31);

	}

	//TODO: DELETE THIS Function
	void TestFormTeam(){
		
//		List<string> zz = new List<string> ();
//		zz.Add ("PlayerOne");
//		zz.Add ("PlayerThree");
//		zz.Add ("PlayerFour");
//		PlayerEvents.TeamUpPlayers (zz);
	}
	

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			eventLock = false;
			QueueEvent (EventType.StraightRockShower);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			eventLock = false;
			QueueEvent (EventType.EnemyEvent);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			eventLock = false;
			QueueEvent (EventType.PointLights);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			eventLock = false;
			QueueEvent (EventType.LittleRockShower);
			QueueEvent (EventType.Blimp);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5)) {
			eventLock = false;
			QueueEvent (EventType.Bomb);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6)) {
			eventLock = false;
			QueueEvent (EventType.MapSpin);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7)) {
			eventLock = false;
			QueueEvent (EventType.HealthPowerUpEvent);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8)) {
			eventLock = false;
			QueueEvent (EventType.BombPowerUpEvent);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9)) {
			eventLock = false;
			QueueEvent (EventType.PointsBounty);
		}else if (Input.GetKeyDown(KeyCode.Alpha0)) {
			eventLock = false;
			QueueEvent (EventType.LeechPowerUpEvent);
		}

		if (CanRunNextEvent ()) {
			StartCoroutine(NextEvent());
		}
	}
	
	public static EventType currentEvent;

	public static List<EventHelper> eventQueue = new List<EventHelper>();

	private static bool eventLock = false;
	

	public static IEnumerator  NextEvent(){

		eventLock = true;
		while (eventQueue.Count == 0) {
//			Debug.Log("No more events to run!");
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
			GameObject obj = Object.Instantiate (Resources.Load ("Events/" + next.type.ToString ())) as GameObject;
			IEvent evt = (IEvent) obj.GetComponent(typeof(IEvent));
			evt.Begin();
		}

		yield return null;
	}
	
	public static void QueueEvent(EventType e,float delay=0){
		eventQueue.Add (new EventHelper(e,delay));
	}

	// pos is in viewport coordinates ( bottom-left is 0,0 and top-Right is 1,1)
	public static void DisplayMessage(string message){
		eventRunner.GetComponent<Message> ().DisplayMessage (message);
	}
	

	// The controller has been notified here that an event has ended. It has also suggested a
	// next event to run and a delay time before running it.
	public static void EventEnd(EventType running, EventType nextState, float delay = 0){
		ExecuteEventTransition (nextState, delay);
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