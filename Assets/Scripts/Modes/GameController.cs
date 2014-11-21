using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameMode mode;

	private GameObject controller;
	private bool started = false;

	void Start()
	{
		controller = GameObject.Find ("GameController");
		//StartGameMode (new TimeLimitMode (30000));
		StartGameMode (new PointsMode (100));
	}

	void Update()
	{
		if (started)
		{
			if (mode.CheckGameOver())
			{
				Pause();
				controller.GetComponent<Message>().DisplayMessage("Game Over!");
				started = false;
			}
		}
	}

	public void Pause()
	{
	}

	public bool StartGameMode(GameMode m)
	{
		mode = m;
		mode.Start ();
		started = true;
		return started;
	}
}