using System;
using System.Collections;
using System.Timers;
using UnityEngine;

public class TimeLimitMode : GameMode
{
	private Timer timer;
	private double timeLeft;
	private bool gameOver;
	private Message message;

	public TimeLimitMode (double time)
	{
		timeLeft = time;

		timer = new Timer ();
		timer.Elapsed += new ElapsedEventHandler(Elapsed);
		timer.Interval = 1000;

		gameOver = false;
	}
	
	public override bool Start()
	{
		// Shouldn't have to do this, but Message isn't done initializing
		GameController.ExecuteOnMainThread.Enqueue (() => {
			DisplayMessage("Time Trial Start!", Color.red);
		});
		timer.Enabled = true;
		return true;
	}

	public static void DisplayMessage(string msg, Color color)
	{
		GameObject.Find ("GameController").GetComponent<Message> ()
			.DisplayMessage (msg, color);
	}

	public override bool CheckGameOver ()
	{
		return gameOver;
	}

	public double TimeLeft()
	{
		return timeLeft;
	}

	private void Elapsed(object source, ElapsedEventArgs e)
	{
		timeLeft -= timer.Interval;
		if (timeLeft <= 0) {
			gameOver = true;
			timer.Enabled = false;
		}

		if (timeLeft == 30000) {
			GameController.ExecuteOnMainThread.Enqueue (() => {
				DisplayMessage("30 seconds left", Color.red);
			});
		} else if (timeLeft == 10000) {
			GameController.ExecuteOnMainThread.Enqueue (() => {
				DisplayMessage("10 seconds left", Color.red);
			});
		} else if (timeLeft < 4000 && timeLeft > 0) {
			GameController.ExecuteOnMainThread.Enqueue (() => {
				DisplayMessage(((int)(timeLeft/1000)).ToString(), Color.red);
			});
		}
	}
}