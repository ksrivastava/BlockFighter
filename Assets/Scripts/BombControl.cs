using UnityEngine;
using System.Collections;

public class BombControl : MonoBehaviour {

	public float bombTimer;

	private GameObject bomb;
	private GUIText timerText;

	void Start () {
		bomb = transform.GetChild (0).gameObject;
		timerText = transform.GetChild (1).guiText;
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
		}
	}

	void UpdateTime() {
		Vector2 bombPos = bomb.transform.position;
		timerText.transform.position = Camera.main.WorldToViewportPoint(bombPos);
		timerText.text = bombTimer.ToString("0.00");
	}
}