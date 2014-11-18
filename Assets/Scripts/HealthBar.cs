using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public float barDisplay;
	public GUIStyle health_full;
	public GUIStyle none_style;

	public float MaxHealth = 100;
	private float health = 100;

	public Texture2D fullHealth;
	public Texture2D halfHealth;
	public Texture2D lowHealth;

	public float Health {
		get {
			return health;
		}
		
		set {
			health = value;
			barDisplay = (health) / MaxHealth;

			if (health <= MaxHealth * 0.7 && health > MaxHealth * 0.3) {
				health_full.normal.background = halfHealth;
			}
			else if (health <= MaxHealth * 0.3) {
				health_full.normal.background = lowHealth;
			}
			else {
				health_full.normal.background = fullHealth;
			}

			// The responsibility of 'dying' is up to the controller of the script
		}
	}

	public bool showDashbar = true;
	public Texture2D dashBarTexture;
	private float dash;
	private float MaxDash = 1;
	public GUIStyle dashBarStyle;

	public float Dash {
		get {
			return dash;
		}
		
		set {
			dash = value;
		}
	}

	Vector2 posRatio = new Vector2(0.06f, 0.03f);
	Vector2 sizeRatio = new Vector2(0.05f, 0.008f);
	string barText = "";

	void Start() {
		Health = MaxHealth;
		Dash = MaxDash;
		dashBarStyle.normal.background = dashBarTexture;
	}
	
	void OnGUI() {
		Vector2 playerSize = this.renderer.bounds.size;
		Vector2 size = new Vector2(sizeRatio.x * Screen.width, sizeRatio.y * Screen.height);

		Vector3 t_pos = transform.position;
		t_pos.y += playerSize.y / 2 + 2.5f;

		Vector2 pos = Camera.main.WorldToScreenPoint (t_pos);

		pos.x -= size.x / 2;
		pos.y += size.y / 2;

		//draw the background:

		if (showDashbar) {
			GUI.BeginGroup(new Rect(pos.x, Screen.height - pos.y, size.x, size.y * 2));
			GUI.Box(new Rect(0,0, size.x * barDisplay, size.y), barText, health_full);
			GUI.Box(new Rect(0, size.y + 1f, size.x * Dash, size.y), barText, dashBarStyle);
			GUI.EndGroup();
		}
		else {
			GUI.BeginGroup(new Rect(pos.x, Screen.height - pos.y, size.x, size.y));
			GUI.Box(new Rect(0, 0, size.x * barDisplay, size.y), barText, health_full);
			GUI.EndGroup();
		}
	}

	float deltaTime;
	float duration = 0.02f;

	void Update() {
		if (deltaTime >= duration) {
			if (Dash < 1) {
				Dash += 0.02f;
			}
			else {
				Dash = 1;
			}
			deltaTime = 0f;
		}
		else {
			deltaTime += Time.deltaTime;
		}
	}

	public void AddHealth(float addedhealth) {
		Health = Mathf.Min (MaxHealth, Health + addedhealth);
	}
}
