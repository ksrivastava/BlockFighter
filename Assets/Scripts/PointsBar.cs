using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DisplayType {
	Point,
	Health,
	Bomb
}

public class PointsBar : MonoBehaviour {

	public GUIStyle[] styles;
	public Font myFont;
	public int myFontSize = 10;
	
	private float scoreFontSize = Screen.height / 25;
	private float playerFontSize = Screen.height / 65;
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
		height = scoreFontSize + 2.5f * playerFontSize;

		yScore = Screen.height - height;
		yPlayer = Screen.height - 1.33f * playerFontSize;

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
			DisplayNumber(obj.transform.GetChild(0).gameObject, p, DisplayType.Point);
		}
	}

	public static float GetPoints(GameObject obj){
		PlayerControl c;
		if (c = obj.GetComponentInChildren<PlayerControl>()) {
			return points[c.GetPlayerNum() - 1];
		}

		return -1;
	}

	public static float[] GetAllPoints() {
		return points;
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
	
	public static void DisplayNumber(GameObject g, float p, DisplayType type) {
		GameObject points = Instantiate(Resources.Load("Points")) as GameObject;

		points.transform.position = g.transform.position;
		if (type == DisplayType.Health) {
			if(p > 0) {
				points.guiText.text = "+" + p.ToString ();
				points.GetComponent<PointsAnimation> ().SetColor (Color.green);
			} else {
				points.guiText.text = p.ToString ();
				points.GetComponent<PointsAnimation> ().SetColor (Color.red);
				points.GetComponent<PointsAnimation> ().SetScale(0.3f);
			}
		} else if (type == DisplayType.Point) {
			if(p > 0) {
				points.guiText.text = "+" + p.ToString () + "P";
				points.GetComponent<PointsAnimation> ().SetColor (Color.yellow);
			} else {
				points.guiText.text = p.ToString () + "P";
				points.GetComponent<PointsAnimation> ().SetColor (Color.magenta);
			}
		} else if (type == DisplayType.Bomb) {
				points.guiText.text = p.ToString ();
				points.GetComponent<PointsAnimation> ().SetColor (Color.red);
				points.GetComponent<PointsAnimation> ().SetAnimationTime(.25f);
				points.GetComponent<PointsAnimation> ().SetScale(0.3f);
		}

		points.GetComponent<PointsAnimation> ().SetGameObject(g);
	}
}