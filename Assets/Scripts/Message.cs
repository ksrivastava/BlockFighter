using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Message : MonoBehaviour {


	List<Vector2> positions = new List<Vector2>();
	static int messagesOnDisplay = 0;
	static int msgIdx = 0;
	int maxMessages = 5;
	float seconds = 3f;

	// Use this for initialization
	void Awake () {
		positions.Add (new Vector2 (0.5f, 0.9f));

		for (int i=1; i <maxMessages; i++) {
			positions.Add(new Vector2(0.5f,positions[positions.Count-1].y - 0.05f));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayMessage(string message){
		StartCoroutine (helper (message, Color.clear));
	}

	public void DisplayMessage(string message, Color color){
		StartCoroutine (helper (message, color));
	}

	private IEnumerator helper(string message, Color color){

		while (messagesOnDisplay >= maxMessages) {
			yield return null;
		}

		messagesOnDisplay = messagesOnDisplay + 1;
		msgIdx = msgIdx + 1;
		msgIdx = msgIdx % maxMessages;


		GameObject obj = Object.Instantiate (Resources.Load ("Message")) as GameObject;
		GUIText guiText = obj.GetComponent<GUIText> ();
		guiText.text = message;
		if (color != Color.clear) {
			guiText.color = color;
		}

		guiText.transform.position = positions[msgIdx];
		MessageLife m = obj.GetComponent<MessageLife> ();
		m.Kill (seconds);

		yield return null;
	}

	public static void Down(){

		messagesOnDisplay = (messagesOnDisplay == 1) ? 0 : messagesOnDisplay - 1;
		msgIdx = (messagesOnDisplay == 0) ? 0 : msgIdx--;
	}
}
