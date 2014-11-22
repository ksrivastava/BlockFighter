using UnityEngine;
using System.Collections;

public class MapSpin : MonoBehaviour, IEvent {

	private GameObject map;
	private float spinAmount = 0f;

	void Start () {
		map = GameObject.Find ("Map");
	}
	
	void Update () {
		if(spinAmount < 360) {
			map.transform.RotateAround (map.transform.position, Vector3.back, 10 * Time.deltaTime);
			spinAmount += 10 * Time.deltaTime;

			if(spinAmount >= 360) {
				spinAmount = 360;
			}
		}
		else {
			map.transform.rotation = Quaternion.Euler(0, 0, 0);
			Destroy(this.gameObject);
		}
	}
	public void Begin(){
		
	}
	
	public void End(){
		EventController.EventEnd (RunnableEventType.MapSpin, RunnableEventType.Idle, 3);
	}
	
	public void OnDestroy(){
		End ();
	}
}
