using UnityEngine;
using System.Collections;

public class HealthPowerUpEvent : MonoBehaviour, IEvent {

	public string path = "HealthPowerUp";

	void Start () {
		var healthPack = Object.Instantiate (Resources.Load (path)) as GameObject;
		Vector2 pos = new Vector2 ();
		pos = healthPack.transform.position;
		pos.x += Random.Range (-20, 20);
		healthPack.transform.position = pos;
	}
	
	public void Begin(){
		
	}
	
	public void End(){
		EventController.EventEnd (EventType.HealthPowerUpEvent,  EventType.Idle, 1);
	}
	
	public void OnDestroy(){
		End ();
	}
}
