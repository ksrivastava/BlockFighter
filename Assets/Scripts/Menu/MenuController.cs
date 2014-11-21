using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public enum Menu {GameModeSelection, GameOver};

	public static Menu menu;

	void Start () {
		switch (menu) {
			case Menu.GameModeSelection:
				GameObject.Find ("GameModeCamera").SetActive(true);
				GameObject.Find ("EndGameCamera").SetActive(false);
				break;
			case Menu.GameOver:
				GameObject.Find ("EndGameCamera").SetActive(true);
				GameObject.Find ("GameModeCamera").SetActive(false);
				break;
		}
	}
}
