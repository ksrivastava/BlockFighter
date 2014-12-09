using UnityEngine;
using System.Collections;

public class StarBar : MonoBehaviour {
	private Animator anim;
	
	void Awake () {
		anim = this.GetComponent<Animator> ();
	}

	void Update () {
		Vector2 pos = this.transform.parent.GetChild(0).position;
		pos.y += 4.8f;
		this.transform.position = pos;
	}

	public void setNumStars(int p) {
		anim.SetInteger ("numStars", p);
	}
}
