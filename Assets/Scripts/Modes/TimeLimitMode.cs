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
		DisplayMessage ("Time Trial Start!");
		timer.Enabled = true;
		return true;
	}

	public static void DisplayMessage(string msg)
	{
		GameObject.Find ("GameController").GetComponent<Message> ()
			.DisplayMessage (msg);
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
				DisplayMessage("30 seconds left");
			});
		} else if (timeLeft == 10000) {
			GameController.ExecuteOnMainThread.Enqueue (() => {
				DisplayMessage("10 seconds left");
			});
		} else if (timeLeft < 4000 && timeLeft > 0) {
			GameController.ExecuteOnMainThread.Enqueue (() => {
				DisplayMessage(((int)(timeLeft/1000)).ToString());
			});
		}
	}
}