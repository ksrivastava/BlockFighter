using UnityEngine;
using System.Collections;

public class EndGameMenuController : MonoBehaviour {

	void Start () {
		float[] points = PointsBar.GetAllPoints ();
		Transform pointsTransform = GameObject.Find ("Points").transform;
		for (int i = 0; i < pointsTransform.childCount; ++i) {
			pointsTransform.GetChild (i).gameObject.GetComponent<TextMesh>().text = points[i].ToString ();
		}
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
			MenuController.menu = MenuController.Menu.GameModeSelection;
			Application.LoadLevel (0);
		}
	}
}
