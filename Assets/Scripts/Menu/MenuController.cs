using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public enum Menu {Title, Help, GameModeSelection, CharacterSelection, MapSelection, GameOver};

	public static Menu menu = Menu.Title;

	void Start () {
		GameObject camera = null;
		switch (menu) {
			case Menu.Title:
				camera = GameObject.Find ("TitleCamera");
				break;
			case Menu.Help:
				camera = GameObject.Find ("HelpCamera");
				break;
			case Menu.GameModeSelection:
				camera = GameObject.Find ("GameModeCamera");
				break;
			case Menu.CharacterSelection:
				camera = GameObject.Find ("CharacterSelectCamera");
				break;
			case Menu.MapSelection:
				camera = GameObject.Find ("MapSelectCamera");
				break;
			case Menu.GameOver:
				camera = GameObject.Find ("EndGameCamera");
				break;
		}
		DisableAllCameras ();
		camera.SetActive (true);
	}

	private void DisableAllCameras() {
		GameObject.Find ("TitleCamera").SetActive(false);
		GameObject.Find ("HelpCamera").SetActive(false);
		GameObject.Find ("GameModeCamera").SetActive(false);
		GameObject.Find ("CharacterSelectCamera").SetActive(false);
		GameObject.Find ("MapSelectCamera").SetActive(false);
		GameObject.Find ("EndGameCamera").SetActive(false);
	}
}
