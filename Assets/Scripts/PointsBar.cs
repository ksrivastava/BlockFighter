using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsBar : MonoBehaviour {

	public GUIStyle[] styles;
	public Font myFont;
	public int myFontSize = 10;
	
	private float scoreFontSize = Screen.height / 15;
	private float playerFontSize = Screen.height / 45;
	private float yScore, yPlayer, xPlayer;
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
		length = Screen.width / 12f;
		height = scoreFontSize + 1.5f * playerFontSize;

		yScore = Screen.height - height;
		yPlayer = Screen.height - 1.33f * playerFontSize;
		//xPlayer = Screen.width / 100;
		xPlayer = 0;

		styles [4].fontSize = (int)scoreFontSize;
		for (int i = 0; i < 4; ++i) {
			styles[i].fontSize = (int)playerFontSize;
			GUI.Box (new Rect((i + 0.5f) * Screen.width / 4.5f, yScore, length, scoreFontSize), points[i].ToString(), styles[4]);
			GUI.Box (new Rect((i + 0.5f) * Screen.width / 4.5f + length / 27f, yPlayer, length, playerFontSize), "Player " + (i + 1).ToString(), styles[i]);
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
