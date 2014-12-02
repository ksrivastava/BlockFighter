using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameMode mode;
	public static List<GameObject> players = new List<GameObject>();

	private GameObject controller;
	private bool started = false;

	public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();


	void Start()
	{
		controller = GameObject.Find ("GameController");
		StartGameMode ();
	}

	void FixedUpdate()
	{
		if (started)
		{
			if (mode.CheckGameOver())
			{
				Pause();
				controller.GetComponent<Message>().DisplayMessage("Game Over!", Color.red);
				StartCoroutine(LoadGameOverScreen());
				started = false;
			}

			while (ExecuteOnMainThread.Count > 0)
			{
				ExecuteOnMainThread.Dequeue().Invoke();
			}
		}
	}

	public void Pause()
	{
		foreach (string p in PlayerEvents.playerNames) {
			var playerController = GameObject.Find (p).GetComponentInChildren<PlayerControl>();
			playerController.allowMovement = false;
			playerController.allowHitting = false;
		}
	}

	public void Unpause()
	{
		foreach (string p in PlayerEvents.playerNames) {
			var playerController = GameObject.Find (p).GetComponentInChildren<PlayerControl>();
			playerController.allowMovement = true;
			playerController.allowHitting = true;
		}
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
		Unpause ();
		Application.LoadLevel (0);
	}
}