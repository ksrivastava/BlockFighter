using UnityEngine;
using System.Collections;

public class EnemyEvent : MonoBehaviour, IEvent {

	private float eventDuration = 30.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Begin() {
		EventController.DisplayMessage("ENEMIES!",2,new Vector2(0.5f,0.5f));
		GameObject e1 = Instantiate (Resources.Load ("Events/Enemy")) as GameObject;
		GameObject e2 = Instantiate (Resources.Load ("Events/Enemy")) as GameObject;

		Vector3 pos = e1.transform.position;
		pos.x = -20.0f;
		pos.y = -10.0f;
		e1.transform.position = pos;

		pos.x = 20.0f;
		e2.transform.position = pos;

		Invoke ("End", eventDuration);
	}

	public void End() {
		EventController.EventEnd (EventType.EnemyEvent, EventType.Idle, 3);
	}

	public void OnDestroy() {
		End ();
	}
}
