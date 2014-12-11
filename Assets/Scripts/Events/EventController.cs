using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController : MonoBehaviour {		
	
	private static GameObject eventRunner;
	
	private static float bombTimer=10;
	private static float leechTimer=10;
	private static float healthTimer=10;
	private static float straightRockSpawnTimer = 10;
	
	private static float chance = 0.25f;
	
	// Use this for initialization
	void Start () {
		
		
		eventRunner = GameObject.Find ("EventRunner");
		
		
		if (Application.loadedLevelName == "_Map_2" || Application.loadedLevelName == "_Map_3") {	
			
			InvokeRepeating("CheckStraightRocks",2,5);
			InvokeRepeating("CheckEnoughBombs",8,13);
			InvokeRepeating("CheckEnoughHealthPacks",16,15);
			InvokeRepeating("CheckEnoughLeeches",12,5);
			InvokeRepeating("CheckBigHammer",15,30);
			
		} else if(Application.loadedLevelName == "_Map_4"){
			//Invoke("QueuePointLights",5);
		}
		
		
		
	}
	
	void CheckStraightRocks(){
		int numStraightRocks = 0;
		foreach (var throwable in GameObject.FindGameObjectsWithTag ("ThrowableObject")) {
			if(throwable.layer == LayerMask.NameToLayer("StraightRock")){
				numStraightRocks++;
			}
		}
		
		if (numStraightRocks == 0) {
			QueueStraightRockShower ();
		}
	}
	
	void QueuePointLights(){
		eventLock = false;
		QueueEvent (RunnableEventType.PointLights);
	}
	
	void QueueStraightRockShower(){
		eventLock = false;
		QueueEvent(RunnableEventType.StraightRockShower);
	}
	
	// Update is called once per frame
	void Update () {
		//
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			eventLock = false;
			QueueEvent (RunnableEventType.StraightRockShower);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			eventLock = false;
			QueueEvent (RunnableEventType.EnemyEvent);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			eventLock = false;
			QueueEvent (RunnableEventType.PointLights);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			eventLock = false;
			QueueEvent (RunnableEventType.LittleRockShower);
			QueueEvent (RunnableEventType.Blimp);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5)) {
			eventLock = false;
			QueueEvent (RunnableEventType.Bomb);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6)) {
			eventLock = false;
			//QueueEvent (RunnableEventType.MapSpin);
			QueueEvent (RunnableEventType.BigHammerPowerUpEvent);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7)) {
			eventLock = false;
			QueueEvent (RunnableEventType.HealthPowerUpEvent);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8)) {
			eventLock = false;
			QueueEvent (RunnableEventType.BombPowerUpEvent);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9)) {
			eventLock = false;
			QueueEvent (RunnableEventType.PointsBounty);
		}else if (Input.GetKeyDown(KeyCode.Alpha0)) {
			eventLock = false;
			QueueEvent (RunnableEventType.LeechPowerUpEvent);
		}
		
		if (CanRunNextEvent ()) {
			StartCoroutine(NextEvent());
		}
	}
	
	public void CheckEnoughBombs(){

		int numBombs = 0;
		foreach (var throwable in GameObject.FindGameObjectsWithTag ("ThrowableObject")) {
			if(throwable.layer == LayerMask.NameToLayer("BombRock")){
				numBombs++;
			}
		}


		if (numBombs == 0) {
			QueueEvent(RunnableEventType.BombPowerUpEvent,0);
			QueueEvent(RunnableEventType.BombPowerUpEvent,2);
		}
	}
	
	public void CheckEnoughHealthPacks(){
		if (GameObject.FindGameObjectsWithTag ("HealthPack").Length == 0) {
			QueueEvent(RunnableEventType.HealthPowerUpEvent,0);
		}
	}

	public void CheckBigHammer(){
		if (GameObject.FindGameObjectsWithTag ("BigHammer").Length == 0) {
			QueueEvent(RunnableEventType.BigHammerPowerUpEvent,0);
		}
	}

	public void CheckEnoughLeeches(){
		
		int numLeechRocks = 0;
		foreach (var throwable in GameObject.FindGameObjectsWithTag ("ThrowableObject")) {
			if(throwable.layer == LayerMask.NameToLayer("LeechRock")){
				numLeechRocks++;
			}
		}
		
		if (numLeechRocks == 0) {
			eventLock = false;
			QueueEvent(RunnableEventType.LeechPowerUpEvent,0);
//			eventLock = false;
//			QueueEvent(RunnableEventType.LeechPowerUpEvent,1);
			eventLock = false;
			QueueEvent(RunnableEventType.LeechPowerUpEvent,2);
		}
	}
	
	public static RunnableEventType currentEvent;
	
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
		
		if (next.type != RunnableEventType.Idle) {
			GameObject obj = Object.Instantiate (Resources.Load ("Events/" + next.type.ToString ())) as GameObject;
			IEvent evt = (IEvent) obj.GetComponent(typeof(IEvent));
			evt.Begin();
		} else if(next.type == RunnableEventType.Idle){
			eventLock = false;
		}
		
		yield return null;
	}
	
	public static void QueueEvent(RunnableEventType e,float delay=0){
		eventQueue.Add (new EventHelper(e,delay));
	}
	
	// pos is in viewport coordinates ( bottom-left is 0,0 and top-Right is 1,1)
	public static void DisplayMessage(string message){
		eventRunner.GetComponent<Message> ().DisplayMessage (message);
	}
	
	
	// The controller has been notified here that an event has ended. It has also suggested a
	// next event to run and a delay time before running it.
	public static void EventEnd(RunnableEventType running, RunnableEventType nextState, float delay = 0){
		ExecuteEventTransition (nextState, delay);
		eventLock = false;
	}
	
	public static void ExecuteEventTransition(RunnableEventType nextState, float delay){
		
		//TODO: encode state machine
		// if(currentEvent == blablabla)
		QueueEvent (nextState, delay);
	}
	
	public static bool CanRunNextEvent(){
		return !eventLock;
	}
	
	public class EventHelper{
		public RunnableEventType type;
		public float delay;
		
		public EventHelper(RunnableEventType t,float d){
			type=t;
			delay=d;
		}
	}
	
}