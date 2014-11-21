using System;

public class LivesMode : GameMode
{
	public LivesMode ()
	{
	}

	public override bool Start()
	{
		return false;
	}

	public override bool CheckGameOver ()
	{
		return false;
	}

}