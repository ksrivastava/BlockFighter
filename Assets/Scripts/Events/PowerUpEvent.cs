using UnityEngine;
using System.Collections;

public class PowerUpEvent : MonoBehaviour, IEvent {
	
	protected string path = null;
	protected EventType eventType = EventType.Idle;

	protected virtual void Start () {
		var powerUp = Object.Instantiate (Resources.Load (path)) as GameObject;
		Vector2 pos = new Vector2 ();
		pos = powerUp.transform.position;
		pos.x += Random.Range (-30, 30);
		powerUp.transform.position = pos;
	}
	
	public void Begin(){
	}
	
	public void End(){
		EventController.EventEnd (eventType,  EventType.Idle, 1);
	}
	
	public void OnDestroy(){
		End ();
	}
}
