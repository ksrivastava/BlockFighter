using UnityEngine;
using System.Collections;

public class PointsAnimation : MonoBehaviour {

	public float speed = 10f;
	public float animationTime = 0.5f;

	private Color col;
	private GameObject g;
	private float y;
	private bool start = false;

	// Update is called once per frame
	void Update () {
		if(start) {
			if (g != null && !g.renderer.enabled) {
				animationTime = 0f;
			}
			Animate ();
			guiText.color = col;
		}
	}
	
	void Animate() {
		if (animationTime > 0) {
			animationTime -= Time.deltaTime;

			if(g == null) return;

			Vector2 pos = g.transform.position;
			pos.y += y;
			y += speed * Time.deltaTime;

			transform.position = Camera.main.WorldToViewportPoint(pos);
		}
		
		if (animationTime < 0) {
			animationTime = 0;
		}

		if (animationTime == 0) {
			Destroy(this.gameObject);
		}
	}

	public void SetColor(Color color) {
		col = color;
	}

	public void SetGameObject(GameObject game) {
		start = true;
		g = game;
	}

	public void SetAnimationTime(float t) {
		animationTime = t;
	}

	public void SetAnimationSpeed(float s) {
		speed = s;
	}

	public void SetScale(float s) {
		Vector3 scale = this.transform.localScale;
		scale.x *= s;
		scale.y *= s;
		scale.z *= s;

		this.transform.localScale = scale;
	}
}
