using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public float barDisplay;
	Vector2 posRatio = new Vector2(0.06f, 0.03f);
	Vector2 sizeRatio = new Vector2(0.2f, 0.02f);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	
	public bool isSecondPlayer = false;

	GameObject player;
	PlayerBehavior behaviour;
	
	void Start() {
		player = GameObject.Find (name + "/Body");
		behaviour = player.GetComponent<PlayerBehavior> ();

		if (!isSecondPlayer) {
			posRatio.x = 1 - posRatio.x - sizeRatio.x;
		}
	}
	
	void OnGUI() {

		float start_x = player.transform.position.x;
		float start_y = player.transform.position.y;

		Vector2 pos = Camera.main.WorldToScreenPoint (new Vector3 (start_x, start_y, 0));

		print (pos);

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
