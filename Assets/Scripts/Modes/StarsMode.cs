using UnityEngine;
using System.Collections;

public class StarsMode : GameMode {
	
	public int starLimit;

	public StarsMode ()
	{
		PointsBar.isStarsMode = true;
	}
	
	public override bool Start()
	{
		this.starLimit = PointsBar.numPlayers;
		if (starLimit == 2) {
			starLimit = 4;
		}

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
