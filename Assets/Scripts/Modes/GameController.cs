using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameMode mode;

	private GameObject controller;
	private bool started = false;

	void Start()
	{
		controller = GameObject.Find ("GameController");
		StartGameMode ();
	}

	void Update()
	{
		if (started)
		{
			if (mode.CheckGameOver())
			{
				Pause();
				controller.GetComponent<Message>().DisplayMessage("Game Over!");
				StartCoroutine(LoadGameOverScreen());
				started = false;
			}
		}
	}

	public void Pause()
	{
	}

	public bool StartGameMode()
	{
		mode.Start ();
		started = true;
		return started;
	}

	private IEnumerator LoadGameOverScreen()
	{
		yield return new WaitForSeconds (2.0f);
		MenuController.menu = MenuController.Menu.GameOver;
		Application.LoadLevel (0);
	}
}