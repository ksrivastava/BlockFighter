using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEvents : MonoBehaviour {

	public static Color teamTwoColorOne = new Color(239,0,255);
		public static Color teamTwoColorTwo = new Color(0,111,255);
	public static Color teamThreeColor = new Color(0,255,247);
	private static Color teamTwoColor;

	//FRIENDLY FIRE CONTROL
	public static bool FriendlyFireOn = false;

	static List<PlayerStats> stats;
	static HeatMap heatmap;
	static HeatTag deathTag;
	static string url = "http://iamkos.com/heatmap.php";
	public bool showDeathmap = false;


	public static List<string> playerNames = new List<string> ();
	

	void Start(){

		teamTwoColor = teamTwoColorOne;
		playerNames.Add ("PlayerOne");
		playerNames.Add ("PlayerTwo");
		playerNames.Add ("PlayerThree");
		playerNames.Add ("PlayerFour");

		heatmap = GetComponent<HeatMap> ();
		deathTag = new HeatTag ("FighterGame-PlayerDeath", url, HeatTag.HeatType.MAP);

		if (showDeathmap) {
			heatmap.PlotData(deathTag);		
		}

		stats = new List<PlayerStats> ();
		foreach (var p in GetAllPlayers()) {
			stats.Add(new PlayerStats(p.transform.parent.name));
		}

	}

	//keep track of player stats OTHER THAN HITS AND DEATHS.
	//TODO: make decisions after collecting the data somewhere. In this class?? TBD.
	void Update(){
	}



	public static void TeamUpPlayers(List<string> playerNames){

		// check that they aren't already in teams
		foreach (var p in playerNames) {
			if(GetPlayerStats(p).onTeam()){
				Debug.LogWarning(p+ " is already on a team! Can't put him on two teams at once.");
				return;
			}
		}
		
		Color color = teamThreeColor;
		if (playerNames.Count == 2) {
			color = teamTwoColor;
			teamTwoColor = (teamTwoColor == teamTwoColorOne) ? teamTwoColorTwo : teamTwoColorOne; 
		} else if (playerNames.Count == 3) {
			color = teamThreeColor;
		}
		
		
		var nameString = "";
		foreach(var p in playerNames){
			
			GetPlayerStats(p).DumpStats();
			foreach(var p2 in playerNames){
				if(p != p2)
					GetPlayerStats(p).AddTeammate(p2);
			}
			nameString+=p+"\n";
			
			// set player color. this has to be done in this way because we're setting active/inactive.
			// maybe there's a way around this?
			GameObject.Find("EventRunner").GetComponent<PlayerEvents>().SetPlayerColor(p,color);
		}
		
		if (nameString == "") {
			Debug.LogError("Couldn't map players to names");
			return;
		}

		EventController.DisplayMessage ("A team has been formed!\n" + nameString);

	}


	public static void DissolveTeam(List<string> playerNames){
		foreach (var p in playerNames) {
			RemovePlayerFromTeam(p);
		}
	}


	// removes the player from his team
	public static void RemovePlayerFromTeam(string player){

		var teammates = new List<string>();
		foreach (var teammate in GetPlayerStats(player).team) {
			teammates.Add(teammate);
		}

		foreach (var teammate in teammates) {
			GetPlayerStats(teammate).RemoveTeammate(player);
			GetPlayerStats(player).RemoveTeammate(teammate);
		}

		if (teammates.Count == 1) {
			EventController.DisplayMessage("A team has been dissolved!");
		}
	}



	public static void TeamBetrayal(){
		foreach (var playerName in playerNames) {
			var attackers = CheckPlayerGangedUpOn(playerName);
			var playerStats = GetPlayerStats(playerName);

			foreach(var attacker in attackers){
				if(playerStats.isTeammate(attacker)){
					EventController.DisplayMessage(attacker+" betrayed "+playerName);
					RemovePlayerFromTeam(attacker);

					//what is the penalty for betraying your teammate?
					PointsBar.AddPoints(GameObject.Find (attacker),-1);

				}
			}
		}
	}

	//returns a list of tuples<playerName,timeOfDeath> if playerName has been ganged up on.
	private static List<Tuple<string,float>> GetPlayerGangUpStatistics(){
		List<Tuple<string,float>> t = new List<Tuple<string,float>>();
		foreach (var playerName in playerNames) {
			var attackers = CheckPlayerGangedUpOn(playerName);
			if(attackers.Count > 1){
				t.Add(new Tuple<string,float>(playerName,GetPlayerStats(playerName).GetLastDeath()));
			}
		}
		return t;
	}
	

	public void SetPlayerColor(string playerName,Color color){
		StartCoroutine (playerColorHelper(playerName,color));
	}
	
	private IEnumerator playerColorHelper(string playerName,Color color){
		GameObject player = GameObject.Find (playerName);
		while (player == null) {
			player = GameObject.Find(playerName);
			yield return null;
		}
		player.GetComponentsInChildren<ColorSetter>()[0].SetColor(color);
	}

	private static List<string> CheckPlayerGangedUpOn(string playerName){
		var playerStats = GetPlayerStats (playerName);

		if (playerStats.deaths == 0) {
			return new List<string>();
		}
		float lookBackDuration = 10f;
		int hitCount = 3; // number of hits before it is considered Ganging up

		// look back lookBackDuration seconds, see if the player has been hit by >1 other players.

		var lastHits = playerStats.GetLastNSecondsHits (lookBackDuration);

		if (lastHits == null || lastHits.Count == 0)
						return new List<string>();

		var attackers = new List<string>();

		foreach(var p in playerNames){
			var x = lastHits.FindAll (d => d.attacker.Equals (p));
			if(x.Count >= hitCount){
				attackers.Add(p);
			}
		}

		return attackers;
	}

	// attacks are hits on players by other players
	public static void RecordAttack(GameObject attackee, GameObject attacker, float damage){
		ModifyStat (attackee.name, AddHit, Time.time, attacker, damage);
	}

	public static void RecordDeath(GameObject dead){
		var lastHit = GetPlayerStats (dead.name).GetLastHit ();
		if (lastHit != null) {
			PointsBar.AddPoints (GameObject.Find (lastHit.attacker), 1);
		} else {
			Debug.Log("no last hit!");
		}


		ModifyStat (dead.name, AddDeath, Time.time);
		heatmap.Post (dead.transform.position, deathTag);
	}

	public static void ProdPlayerWithHighestHealth(){
		var playerName = GetPlayerWithHighestHealth ().transform.parent.name;
		EventController.DisplayMessage (playerName + " is doing quite well...");
	}

	public static GameObject GetPlayerWithHighestHealth(){
		GameObject player = null;
		float health = 0;

		foreach (var p in GetAllPlayers()) {
			var h = GetPlayerHealth(p);
			if( h > health){
				health = h;
				player = p;
			} 
		}
		return player;
	}

	public static float GetPlayerHealth(GameObject player){
		return player.GetComponent<PlayerBehavior>().healthBar.Health;
	}

	public static List<GameObject> GetAllPlayers(){
		List<GameObject> playerList = new List<GameObject>();
		var pl = GameObject.FindGameObjectsWithTag("Player");
		foreach (var p in pl) {
			playerList.Add(p.gameObject);
		}
		return playerList;
	}


	
	// helper stuff for tracking stats
	
	public static PlayerStats GetPlayerStats(string playerName){
		foreach (var p in stats) {
			if(p.playerName == playerName){
				return p;
			}
		}
		return null;
	}
	
	public delegate void Func(params object[] values);
	
	private static void ModifyStat(string playerName, Func f, params object[] values){
		foreach (var s in stats) {
			if(s.playerName == playerName){
				f(s,values);
				return;
			}
		}
	}
	
	//TODO: there has GOT to be a cleaner way of accessing these things.
	private static void AddDeath(params object[] values){
		PlayerStats p = values [0] as PlayerStats;
		float time = (float)(values [1] as object[])[0];
		p.deaths++;
		p.deathTimes.Add (time);
	}
	
	private static void AddHit(params object[] values){
		PlayerStats p = values [0] as PlayerStats;
		var valuesTwo = values [1] as object[];
		var time = (float)valuesTwo [0];
		var attacker = valuesTwo[1] as GameObject;
		var damage = (float)valuesTwo[2];
		p.Hit (attacker.name, damage, time);
	}
}