using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsBar : MonoBehaviour {
	
	public float xScale, yScale;
	public GUIStyle[] styles;
	
	public Texture2D p1Texture;
	public Texture2D p2Texture;
	public Texture2D p3Texture;
	public Texture2D p4Texture;

	private float x, y;
	private float length, height;

	private static float[] points;
	private static float total = 16.0f;

	private Rect barRect;

	void Start() {
		styles = new GUIStyle[4];
		points = new float[4];

		for (int i = 0; i < 4; ++i) {
			points[i] = 4;
		}

		for (int i = 0; i < styles.Length; ++i) {
			styles[i] = new GUIStyle();
			styles[i].alignment = TextAnchor.MiddleCenter;
		}

		styles[0].normal.background = p1Texture;
		styles[1].normal.background = p2Texture;
		styles[2].normal.background = p3Texture;
		styles[3].normal.background = p4Texture;
	}

	void OnGUI() {
		length = yScale * Screen.width;
		height = xScale * 10;
		
		x = (Screen.width - length) / 2;
		y = 10;

		barRect = new Rect (x, y, length, height);
		GUI.Box (barRect, "");

		float last = 0;
		for (int i = 0; i < 4; ++i) {
			GUI.Box (new Rect(barRect.xMin + last * length, y, length * (points[i] / total), height),
			         (i + 1).ToString(), styles[i]);
			last += (points[i] / total);
		}
	}

	public static void AddPoints(GameObject obj, float p) {
		PlayerControl c;
		if (c = obj.GetComponentInChildren<PlayerControl>()) {
			points[c.GetPlayerNum() - 1] += p;
			total += p;
		}
	}

}
