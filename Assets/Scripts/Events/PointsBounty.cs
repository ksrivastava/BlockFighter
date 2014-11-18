using UnityEngine;
using System.Collections;

public class PointsBounty : MonoBehaviour, IEvent {
	
	PlayerBehavior bountyPlayerBehaviour;
	string bountyPlayerName;
	float bounty;

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

		EventController.DisplayMessage("There is a "+maxPoints+" bounty on "+maxPlayerName);

		// begin waiting for player death
		StartCoroutine (ListenForDeath ());

	}
	
	public void End(){
		//EventController.EventEnd (EventType.PointsBounty,  EventType.Idle, 1);
		EventController.DisplayMessage ("Bounty has been gained!");
	}
	
	public void OnDestroy(){
		End ();
	}

	public IEnumerator ListenForDeath(){
		while (bountyPlayerBehaviour.active) {
			yield return null;
		}
		End ();
	}
}
