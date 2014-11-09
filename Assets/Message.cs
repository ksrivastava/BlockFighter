using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayMessage(string message,float seconds,Vector2 pos, float startTime=0){
		StartCoroutine (helper (message, seconds, pos, startTime));
	}

	private IEnumerator helper(string message,float seconds,Vector2 pos, float startTime=0){
		
		while(Time.time < startTime) {
			yield return null;
		}
		GameObject obj = Object.Instantiate (Resources.Load ("Message")) as GameObject;
		GUIText guiText = obj.GetComponent<GUIText> ();
		guiText.text = message;
		guiText.transform.position = pos;
		MessageLife m = obj.GetComponent<MessageLife> ();
		m.Kill (seconds);
	}
}
