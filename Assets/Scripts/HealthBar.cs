using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public float barDisplay;

	public float MaxHealth = 100;
	private float health = 100;

	public float Health {
		get {
			return health;
		}
		
		set {
			health = value;
			barDisplay = (health) / MaxHealth;

			// The responsibility of 'dying' is up to the controller of the script
		}
	}
	
	Vector2 posRatio = new Vector2(0.06f, 0.03f);
	Vector2 sizeRatio = new Vector2(0.2f, 0.02f);
	public Texture2D emptyTex;
	public Texture2D fullTex;

	void Start() {

	}
	
	void OnGUI() {
		Vector2 playerSize = this.renderer.bounds.size;
		Vector2 size = new Vector2(sizeRatio.x * Screen.width, sizeRatio.y * Screen.height);

		Vector3 t_pos = transform.position;
		t_pos.y += playerSize.y / 2 + 2f;

		Vector2 pos = Camera.main.WorldToScreenPoint (t_pos);

		pos.x -= size.x / 2;
		pos.y += size.y / 2;

		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, Screen.height - pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
		GUI.EndGroup();
		
		GUI.EndGroup();
	}
	
	void Update() {

	}
}
