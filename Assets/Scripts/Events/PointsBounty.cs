using UnityEngine;
using System.Collections;

public class PointsBounty : MonoBehaviour, IEvent {
	
	static PlayerBehavior bountyPlayerBehaviour;
	static string bountyPlayerName;
	static float bounty;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Begin(){
		// pick the player who is doing the best
		// declare a points bounty on their head
		// wait for death

		float maxPoints = 0;
		string maxPlayerName = "";
		foreach (var playerName in PlayerEvents.playerNames) {
			var points = PointsBar.GetPoints(GameObject.Find(playerName));
			if(points > maxPoints){
				maxPoints = points;
				maxPlayerName = playerName;
				bountyPlayerBehaviour = GameObject.Find(playerName).GetComponentInChildren<PlayerBehavior>();
				bountyPlayerName = playerName;
				bounty = maxPoints;
			}
		}
		if (maxPoints == 0 || maxPlayerName == "")
						return;

		bounty = Mathf.Floor (maxPoints / 2);
		EventController.DisplayMessage("There is a "+bounty+" point bounty on "+maxPlayerName);

		// begin waiting for player death
		StartCoroutine (ListenForDeath ());

	}
	
	public void End(){
	}
	
	public void OnDestroy(){
	}

	public IEnumerator ListenForDeath(){
		while (bountyPlayerBehaviour != null && bountyPlayerBehaviour.active) {
			yield return null;
		}
		End ();
	}

	public static void BountyWinner(GameObject winner){
		if (winner == null)
						return;
		PointsBar.AddPoints (winner, bounty);
		PointsBar.AddPoints (bountyPlayerBehaviour.transform.parent.gameObject, -bounty);
		EventController.DisplayMessage(winner.name+" has taken "+bounty+" points from "+bountyPlayerName);
	}
}
