using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsBar : MonoBehaviour {

	public GUIStyle[] styles;
	public Font myFont;
	public int myFontSize = 10;

	private float y;
	private float length, height;

	private static float[] points;
	private static float total = 16.0f;

	void Start() {
		styles = new GUIStyle[5];
		points = new float[4];

		GameObject[] pl = GameObject.FindGameObjectsWithTag("Player");

		for (int i = 0; i < 4; ++i) {
			points[i] = 0;
			styles[i] = new GUIStyle();
			styles[i].alignment = TextAnchor.MiddleCenter;
			styles[i].font = myFont;
			styles[i].fontSize = myFontSize;
		}

		for (int i = 0; i < 4; ++i) {
			PlayerControl c = pl[i].GetComponent<PlayerControl>();
			styles[c.GetPlayerNum() - 1].normal.textColor = pl[i].GetComponent<ColorSetter>().color;
		}

		styles[4] = new GUIStyle ();
		styles[4].alignment = TextAnchor.MiddleCenter;
		styles[4].font = myFont;
		styles[4].fontSize = 30;
		styles[4].normal.textColor = Color.white;
	}

	void OnGUI() {
		length = 40;
		height = 40;

		y = Screen.height - height - 14;

		for (int i = 0; i < 4; ++i) {
			GUI.Box (new Rect((i + 1) * Screen.width / 5f - length / 2, y, length, height), points[i].ToString(), styles[4]);
			GUI.Box (new Rect((i + 1) * Screen.width / 5f - length / 2 - 10, y + 25, length + 25, height), "Player " + (i + 1).ToString(), styles[i]);
		}
	}

	public static void AddPoints(GameObject obj, float p) {
		PlayerControl c;
		if (c = obj.GetComponentInChildren<PlayerControl>()) {
			points[c.GetPlayerNum() - 1] += p;
			total += p;
			//PrintAddedPoint(c, p, Color.yellow);
		}
	}

	public static void PrintAddedPoint(PlayerControl c, float p, Color col) {
		GameObject points = Instantiate(Resources.Load("Points")) as GameObject;
		points.GetComponent<PointsAnimation> ().SetColor (col);
		points.GetComponent<PointsAnimation> ().SetPlayer (c);
		if(p > 0) {
			points.guiText.text = "+" + p.ToString ();
		} else if (p < 0) {
			points.guiText.text = "-" + p.ToString ();
		}
		//points.transform.position = Camera.main.WorldToViewportPoint (pointsPos[c.GetPlayerNum() - 1]);
	}

	public static float GetPoints(GameObject obj){
		PlayerControl c;
		if (c = obj.GetComponentInChildren<PlayerControl>()) {
			return points[c.GetPlayerNum() - 1];
		}

		return -1;
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
