using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour, IEvent {

	public float bombTimer;

	private GameObject bomb;
	private GUIText timerText;

	void Start () {
	}

	void Update () {
		UpdateTime ();
		Timer ();
	}

	void Timer() {
		if (bombTimer > 0) {
			bombTimer -= Time.deltaTime;
		}

		if (bombTimer < 0) {
			bombTimer = 0;
		}

		if (bombTimer == 0) {
			UpdateTime ();
			GameObject playerOne = GameObject.Find("PlayerOne");
			GameObject playerTwo = GameObject.Find("PlayerTwo");
			Destroy(playerOne);
			Destroy(playerTwo);
			//EXPLODE
			Destroy(this.gameObject);
		}
	}

	void UpdateTime() {
		if (bomb == null)
			return;
		Vector2 bombPos = bomb.transform.position;
		timerText.transform.position = Camera.main.WorldToViewportPoint(bombPos);
		timerText.text = bombTimer.ToString("0.00");
	}

	
	public void Begin(){
		bomb = transform.GetChild (0).gameObject;
		timerText = transform.GetChild (1).guiText;
	}

	//TODO: Implement end logic. How do you decide what event to call next??
	public void End(){
		EventController.EventEnd (RunnableEventType.Bomb, RunnableEventType.Idle, 1);
	}

	public void OnDestroy(){
		End ();
	}
}