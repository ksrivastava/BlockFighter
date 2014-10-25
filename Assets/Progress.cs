using UnityEngine;
using System.Collections;

public class Progress : MonoBehaviour {
	public float barDisplay;
	Vector2 posRatio = new Vector2(0.06f, 0.03f);
	Vector2 sizeRatio = new Vector2(0.15f, 0.05f);
	public Texture2D emptyTex;
	public Texture2D fullTex;

	public bool isSecondPlayer = false;
	PlayerBehavior behaviour;

	void Start() {
		behaviour = GameObject.Find (name + "/Body").GetComponent<PlayerBehavior> ();
		if (!isSecondPlayer) {
			posRatio.x = 1 - posRatio.x - sizeRatio.x;
		}
	}

	void OnGUI() {

		Vector2 pos = new Vector2(posRatio.x * Screen.width, posRatio.y * Screen.height);
		Vector2 size = new Vector2(sizeRatio.x * Screen.width, sizeRatio.y * Screen.height);

		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
			GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
			GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
					GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
			GUI.EndGroup();

		GUI.EndGroup();
	}
	
	void Update() {
		barDisplay = (behaviour.Health) / behaviour.MaxHealth;
	}
}
