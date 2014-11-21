using System;
using UnityEngine;

public class PointsMode : GameMode
{
	private int pointLimit;

	public PointsMode (int pointLimit)
	{
		this.pointLimit = pointLimit;
	}

	public override bool Start()
	{
		GameObject.Find ("GameController").GetComponent<Message> ()
			.DisplayMessage ("Deathmatch Start!");
		return true;
	}

	public override bool CheckGameOver ()
	{
		float[] points = PointsBar.GetAllPoints ();
		bool gameOver = false;
		for (int i = 0; i < points.Length; ++i)
		{
			if (points[i] >= pointLimit)
			{
				points[i] = pointLimit;
				gameOver = true;
			}
		}
		return gameOver;
	}

}