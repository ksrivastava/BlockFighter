using UnityEngine;
using System.Collections;

public class PointsBounty : MonoBehaviour, IEvent {
	
	static PlayerBehavior bountyPlayerBehaviour;
	static string bountyPlayerName;
	static float bounty;

	static bool collected = false;
	float bountyExpireTime = 30;
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

		EventController.DisplayMessage ("There is a " + bounty + " point bounty on " + maxPlayerName);


		// begin waiting for player death
		StartCoroutine (ListenForDeath ());
		Invoke ("CollectWarning", 2);
		Invoke ("AwardToPlayer", bountyExpireTime);
	}

	public void CollectWarning(){
		EventController.DisplayMessage("Collect it within "+bountyExpireTime+" seconds or they will get away with it!");
	}
	
	public void End(){
		//print ("End!");
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
		//print ("Winner is " + winner.name);
		if (winner == null || collected)
						return;
		PointsBar.AddPoints (winner, bounty);
		EventController.DisplayMessage(winner.name+" has taken the bounty! "+bounty+" points");
		collected = true;
	}
}
