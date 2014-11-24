using UnityEngine;
using System.Collections;

public class PowerUpEvent : MonoBehaviour, IEvent {
	
	protected string path = null;
	protected RunnableEventType eventType = RunnableEventType.Idle;

	protected virtual void Start () {
		var powerUp = Object.Instantiate (Resources.Load (path)) as GameObject;

		var itemSpawnCamera = GameObject.Find ("ItemSpawnCamera");

		Vector3 screenPosition = itemSpawnCamera.camera.ScreenToWorldPoint(new Vector3(Random.Range(0,Screen.width), Random.Range(0,Screen.height), 10));
		powerUp.transform.position = screenPosition;
		Destroy (this.gameObject);
	}
	
	public void Begin(){
	}
	
	public void End(){
		EventController.EventEnd (eventType,  RunnableEventType.Idle, 1);
	}
	
	public void OnDestroy(){
		End ();
	}
}
