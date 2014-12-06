using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameMode mode;
	public static Dictionary<int, Character> chars =
		new Dictionary<int, Character>();

	private GameObject controller;
	private bool started = false;

	public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();


	void Start()
	{
		controller = GameObject.Find ("GameController");

		foreach (int p in chars.Keys) {
			GameObject obj = null;
			switch (chars[p]) {
				case Character.Elf:
				obj = Instantiate(Resources.Load ("Elf")) as GameObject;
					break;
				case Character.Human:
				obj = Instantiate(Resources.Load ("Human")) as GameObject;
					break;
				case Character.Troll:
				obj = Instantiate(Resources.Load ("Troll")) as GameObject;
					break;
				case Character.Orc:
				obj = Instantiate(Resources.Load ("Orc")) as GameObject;
					break;
				case Character.Skeleton:
				obj = Instantiate(Resources.Load ("Skeleton")) as GameObject;
					break;
				case Character.Pig:
					obj = Instantiate(Resources.Load ("Pig")) as GameObject;
					break;
			}
			obj.GetComponentInChildren<PlayerControl>().playerNum = p + 1;
			if (p == 0) {
				obj.name = "PlayerOne";
				SetChildrenLayer(obj, 9);

			} else if (p == 1) {
				obj.name = "PlayerTwo";
				SetChildrenLayer(obj, 10);

			}  else if (p == 2) {
				obj.name = "PlayerThree";
				SetChildrenLayer(obj, 14);

			}  else if (p == 3) {
				obj.name = "PlayerFour";
				SetChildrenLayer(obj, 15);

			}
		}
		Instantiate(Resources.Load ("PointsBar"));
		PointsBar.numPlayers = chars.Keys.Count;
		GameObject eventRunner = Instantiate(Resources.Load ("EventRunner")) as GameObject;
		eventRunner.name = "EventRunner";

		StartGameMode ();
	}

	void SetChildrenLayer(GameObject obj, int layerNum) {
		Transform[] ts = obj.GetComponentsInChildren<Transform>();
		foreach (Transform child in ts) {
			child.gameObject.layer = layerNum;
		}
	}

	void FixedUpdate()
	{
		if (started)
		{
			if (mode.CheckGameOver())
			{
				Pause();
				controller.GetComponent<Message>().DisplayMessage("Game Over!", Color.red);
				StartCoroutine(LoadGameOverScreen());
				started = false;
			}

			while (ExecuteOnMainThread.Count > 0)
			{
				ExecuteOnMainThread.Dequeue().Invoke();
			}
		}
	}

	public void Pause()
	{
		int num = PointsBar.numPlayers;
		foreach (string p in PlayerEvents.playerNames) {
			var playerController = GameObject.Find (p).GetComponentInChildren<PlayerControl>();
			playerController.allowMovement = false;
			playerController.allowHitting = false;
			num--;
			if (num == 0) {
				break;
			}
		}
	}

	public void Unpause()
	{
		int num = PointsBar.numPlayers;
		foreach (string p in PlayerEvents.playerNames) {
			var playerController = GameObject.Find (p).GetComponentInChildren<PlayerControl>();
			playerController.allowMovement = true;
			playerController.allowHitting = true;
			num--;
			if (num == 0) {
				break;
			}
		}
	}

	public bool StartGameMode()
	{
		mode.Start ();
		started = true;
		return started;
	}

	private IEnumerator LoadGameOverScreen()
	{
		yield return new WaitForSeconds (2.0f);
		MenuController.menu = MenuController.Menu.GameOver;
		Unpause ();
		Application.LoadLevel (0);
	}
}