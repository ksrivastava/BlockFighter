using System;
using UnityEngine;

public class LivesMode : GameMode
{
	public LivesMode ()
	{
	}

	public override bool Start()
	{
		GameObject.Find ("GameController").GetComponent<Message> ()
			.DisplayMessage ("Elimination Start!");
		return false;
	}

	public override bool CheckGameOver ()
	{
		return false;
	}

}