using UnityEngine;
using System.Collections;

public class PointsAnimation : MonoBehaviour {

	public float speed = 10f;
	public float animationTime = .5f;

	private Color col;
	private PlayerControl c;
	private float y;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Animate ();
		guiText.color = col;
	}
	
	void Animate() {
		if (animationTime > 0) {
			animationTime -= Time.deltaTime;

			// GET POS OF PLAYER
			y += speed * Time.deltaTime;
			// ADD Y TO PLAYER Y

			//Vector2 pos = Camera.main.ViewportToWorldPoint(transform.position);
			//pos.y += speed * Time.deltaTime;
			//transform.position = Camera.main.WorldToViewportPoint(pos);
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

	public void SetPlayer(PlayerControl player) {
		c = player;
	}
}
