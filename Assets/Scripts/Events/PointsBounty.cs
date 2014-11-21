using UnityEngine;
using System.Collections;

public class PointsBounty : MonoBehaviour, IEvent {
	
	static PlayerBehavior bountyPlayerBehaviour;
	static string bountyPlayerName;
	static float bounty;

	static bool collected = false;
	float bountyExpireTime = 60;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (collected) {
			End ();
		}
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

		bounty = Mathf.Floor (maxPoints / 2);

		if (maxPoints == 0 || maxPlayerName == "" || bounty == 0) {
			End ();
			return;
		}

		EventController.DisplayMessage("There is a "+bounty+" point bounty on "+maxPlayerName);

		// begin waiting for player death
		StartCoroutine (ListenForDeath ());
		Invoke ("AwardToPlayer", bountyExpireTime);
	}
	
	public void End(){
		Destroy (this.gameObject);
	}
	
	public void OnDestroy(){
	}

	public IEnumerator ListenForDeath(){
		while (bountyPlayerBehaviour != null && bountyPlayerBehaviour.active) {
			yield return null;
		}
		End ();
	}

	public void AwardToPlayer(){
		PointsBar.AddPoints (bountyPlayerBehaviour.transform.parent.gameObject, bounty);
		EventController.DisplayMessage (bountyPlayerName + " got away with the bounty!");
		collected = true;

	}

	public static void BountyWinner(GameObject winner){
		if (winner == null || collected)
						return;
		PointsBar.AddPoints (winner, bounty);
		PointsBar.AddPoints (bountyPlayerBehaviour.transform.parent.gameObject, -bounty);
		EventController.DisplayMessage(winner.name+" has taken "+bounty+" points from "+bountyPlayerName);
		collected = true;
	}
}
