using UnityEngine;
using System.Collections;

public class PlayerNumber : MonoBehaviour {

	private Transform parent;
	void Start () {
		parent = transform.parent.transform;
		PlayerControl player = (PlayerControl)parent.GetComponent (typeof(PlayerControl));
		guiText.text = player.GetPlayerNum ().ToString();
	}
	
	void Update () {	
		transform.position = Camera.main.WorldToViewportPoint(new Vector2(parent.position.x, parent.position.y));
	}
}
