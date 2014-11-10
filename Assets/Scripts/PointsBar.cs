using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsBar : MonoBehaviour {
	
	public float xScale, yScale;
	public GUIStyle[] styles;
	public Font myFont;
	public int myFontSize = 10;

	private float x, y;
	private float length, height;

	private static float[] points;
	private static float total = 16.0f;

	private Rect barRect;

	void Start() {
		styles = new GUIStyle[4];
		points = new float[4];

		GameObject[] pl = GameObject.FindGameObjectsWithTag("Player");

		for (int i = 0; i < 4; ++i) {
			points[i] = 4;
			styles[i] = new GUIStyle();
			styles[i].alignment = TextAnchor.MiddleCenter;
			styles[i].font = myFont;
			styles[i].fontSize = myFontSize;
		}

		for (int i = 0; i < 4; ++i) {
			PlayerControl c = pl[i].GetComponent<PlayerControl>();
			Texture2D tex = MakeTexture (100,100,pl[i].GetComponent<ColorSetter>().color);
			styles[c.GetPlayerNum() - 1].normal.background = tex;
		}
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
//		PlayerControl c;
//		if (c = obj.GetComponentInChildren<PlayerControl>()) {
//			points[c.GetPlayerNum() - 1] += p;
//			total += p;
//		}
	}

	private Texture2D MakeTexture(int width, int height, Color col) {
		col.a = 1;
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[i] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}
	
}
