using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public enum DisplayType {
	Point,
	Health,
	Bomb
}

public class PointsBar : MonoBehaviour {

	const int MAX_PLAYERS = 4;
	public GUIStyle[] styles;
	public Font myFont;
	public int myFontSize = 10;
	
	private float scoreFontSize = Screen.height / 25;
	private float playerFontSize = Screen.height / 65;
	private float yScore, yPlayer, xPlayer;
	private float length, height;
	
	private string[] names;
	private GameObject[] players;
	private static float[] points;
	public static bool isStarsMode = false;
	public static int numPlayers = 2;
	
	void Start() {
		float initPoints = 0f;
		GameController.mode.numPlayers = numPlayers;

		if (isStarsMode) {
			if (numPlayers == 2) {
				initPoints = 2f;
			}
			else {
				initPoints = 1f;
			}
		}

		styles = new GUIStyle[numPlayers + 1];
		points = new float[numPlayers];
		names = new string[numPlayers];
		
		players = GameObject.FindGameObjectsWithTag("Player");
		
		for (int i = 0; i < numPlayers; ++i) {
			points[i] = initPoints;
			styles[i] = new GUIStyle();
			styles[i].alignment = TextAnchor.MiddleCenter;
			styles[i].font = myFont;
			styles[i].fontSize = myFontSize;
			if(isStarsMode) {
				players[i].transform.parent.gameObject.GetComponentInChildren<StarBar>().setNumStars((int)initPoints);
			}
		}
		
		for (int i = 0; i < numPlayers; ++i) {
			PlayerControl c = players[i].GetComponent<PlayerControl>();
			SpriteRenderer s = players[i].GetComponent<SpriteRenderer>();
			styles[c.GetPlayerNum() - 1].normal.textColor = players[i].GetComponent<ColorSetter>().color;
			names[c.GetPlayerNum() - 1] = c.GetName();
		}
		
		styles[numPlayers] = new GUIStyle ();
		styles[numPlayers].alignment = TextAnchor.MiddleCenter;
		styles[numPlayers].font = myFont;
		styles[numPlayers].fontSize = 30;
		styles[numPlayers].normal.textColor = Color.white;
	}

	public static Texture2D textureFromSprite(Sprite sprite)
	{
		/* By user: trothmaster 
		 * Found at: http://answers.unity3d.com/questions/651984/convert-sprite-image-to-texture.html
		 */

		if(sprite.rect.width != sprite.texture.width){
			Texture2D newText = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
			Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, 
			                                             (int)sprite.textureRect.y, 
			                                             (int)sprite.textureRect.width, 
			                                             (int)sprite.textureRect.height );
			newText.SetPixels(newColors);
			newText.Apply();
			return newText;
		} else
			return sprite.texture;
	}
	
	Texture2D getSprite(int playerNum) {
		Sprite sprite = players [playerNum].GetComponent<SpriteRenderer>().sprite;
		string spriteSheet = AssetDatabase.GetAssetPath( sprite.texture );
		Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath( spriteSheet ).OfType<Sprite>().ToArray();
		var spriteIndex = 3;
		if (sprite.name.Contains("Pig")) {
			spriteIndex = 1;
		}
		Texture2D image = textureFromSprite (sprites [spriteIndex]);
		return image;
	}

	void OnGUI() {
		length = Screen.width / 12f;
		height = scoreFontSize + 1.8f * playerFontSize;
		
		yScore = Screen.height - height;
		yPlayer = Screen.height - 1.333f * playerFontSize;
		
		styles [numPlayers].fontSize = (int)scoreFontSize;
		for (int i = 0; i < numPlayers; ++i) {
			styles[i].fontSize = (int)playerFontSize;
			var tempX = ((i + 0.5f) + (MAX_PLAYERS - numPlayers) * 0.5f ) * Screen.width / 4.5f;
			GUI.Box (new Rect(tempX, yScore, length, scoreFontSize), points[i].ToString(), styles[numPlayers]);
			GUI.Box (new Rect(tempX + length / 27f, yPlayer, length, playerFontSize), names[i], styles[i]);
//			Texture2D image = getSprite(i);
//			int scaleMult = 3;
//			TextureScale.Point (image, image.width * scaleMult, image.height * scaleMult);
//			GUI.Box (new Rect(tempX - 35f, yScore, image.width, image.height), image, styles[numPlayers]);
		}
	}

	public static void AddPoints(GameObject obj, float p) {
		if (!isStarsMode) {
			PlayerControl c;
			if (c = obj.GetComponentInChildren<PlayerControl>()) {
				points[c.GetPlayerNum() - 1] += p;
				DisplayNumber(obj.transform.GetChild(0).gameObject, p, DisplayType.Point);
			}
		}
	}
	
	public static void AddStars(GameObject obj) {
		PlayerControl c;
		if (c = obj.GetComponentInChildren<PlayerControl>()) {
			points[c.GetPlayerNum() - 1] += 1;
			if(isStarsMode) {
				obj.transform.parent.gameObject.GetComponentInChildren<StarBar>().setNumStars((int)points[c.GetPlayerNum() - 1]);
			}
		}
	}
	
	public static void RemoveStars(GameObject obj, Vector3 starSpawnPosition) {
		if (isStarsMode) {
			PlayerControl c;
			if (c = obj.GetComponentInChildren<PlayerControl>()) {
				if (points[c.GetPlayerNum() - 1] > 0) {
					points[c.GetPlayerNum() - 1] -= 1;
					obj.GetComponentInChildren<StarBar>().setNumStars((int)points[c.GetPlayerNum() - 1]);
					var star = Object.Instantiate (Resources.Load ("PowerUp/StarPowerUp")) as GameObject;
					star.transform.position = starSpawnPosition;
				}
			}
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

	public static void Clear() {
		isStarsMode = false;
		points = new float[numPlayers];
	}
}