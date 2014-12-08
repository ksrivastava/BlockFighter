using System;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
	public int numPlayers;
	public GameMode ()
	{
	}

	public abstract bool Start();
	public abstract bool CheckGameOver();
}