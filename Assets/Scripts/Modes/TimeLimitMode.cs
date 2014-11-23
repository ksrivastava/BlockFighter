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
		GameObject.Find ("GameController").GetComponent<Message> ()
			.DisplayMessage ("Time Trial Start!");
		timer.Enabled = true;
		return true;
	}

	public override bool CheckGameOver ()
	{
		if (timeLeft == 30000) {
			GameObject.Find ("GameController").GetComponent<Message> ()
				.DisplayMessage ("30 seconds left");
		} else if (timeLeft < 10000 && timeLeft > 0) {
			GameObject.Find ("GameController").GetComponent<Message> ()
				.DisplayMessage (((int)(timeLeft/1000)).ToString());
		}
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
	}
}