﻿using UnityEngine;
using System.Collections;

public class StarsMode : GameMode {
	
	private int starLimit;

	public StarsMode ()
	{
		this.starLimit = PointsBar.numPlayers;
		PointsBar.isStarsMode = true;
	}
	
	public override bool Start()
	{
		DisplayMessage("Get " + starLimit + " stars to win!", Color.red);
		return true;
	}
	
	public static void DisplayMessage(string msg, Color color)
	{
		GameObject.Find ("GameController").GetComponent<Message> ()
			.DisplayMessage (msg, color);
	}
	
	public override bool CheckGameOver ()
	{
		float[] points = PointsBar.GetAllPoints ();
		bool gameOver = false;
		for (int i = 0; points != null && i < points.Length; ++i)
		{
			if (points[i] >= starLimit)
			{
				points[i] = starLimit;
				gameOver = true;
			}
		}
		return gameOver;
	}
}
