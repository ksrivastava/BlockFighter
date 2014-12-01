using UnityEngine;
using System.Collections;

public class StarsMode : GameMode {

	private int numStars;
	private int starLimit;
	
	public StarsMode (int numStars = 3, int starLimit = 4)
	{
		this.numStars = numStars;
		this.starLimit = starLimit;
	}
	
	public override bool Start()
	{
		DisplayMessage("Get " + numStars + " stars!", Color.red);
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
		for (int i = 0; i < points.Length; ++i)
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
