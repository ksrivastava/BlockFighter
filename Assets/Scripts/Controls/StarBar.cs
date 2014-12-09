using UnityEngine;
using System.Collections;

public class StarBar : MonoBehaviour {
	private Animator anim;
	
	void Start () {
		if (!PointsBar.isStarsMode) {
			Destroy(this.gameObject);
		}
		anim = this.GetComponent<Animator> ();
	}

	void Update () {
		Vector2 pos = this.transform.parent.GetChild(0).position;
		pos.y += 4.6f;
		this.transform.position = pos;
	}

	public void setNumStars(int p) {
		anim.SetInteger ("numStars", p);
	}
}
