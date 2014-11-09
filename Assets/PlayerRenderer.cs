using UnityEngine;
using System.Collections;

public class PlayerRenderer : MonoBehaviour {

	PlayerControl controller;
	PlayerBehavior behavior;
	GUIText guiText;

	// Use this for initialization
	void Start () {
		controller = GetComponent<PlayerControl> ();
		behavior = GetComponent<PlayerBehavior> ();

		GameObject obj = Object.Instantiate (Resources.Load ("Message")) as GameObject;
		guiText = obj.GetComponent<GUIText> ();
		guiText.text = controller.playerNum.ToString();
	}

	// Update is called once per frame
	void Update () {
		var pos = transform.position;
//		pos.x += 2f;
		guiText.transform.position = pos;
	}
}
