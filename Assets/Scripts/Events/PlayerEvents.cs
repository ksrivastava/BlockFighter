using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEvents : MonoBehaviour {

	static List<PlayerStats> stats;
	static HeatMap heatmap;
	static HeatTag deathTag;
	static string url = "http://iamkos.com/heatmap.php";
	public bool showDeathmap = false;
	static GameObject eventRunner;

	public static List<string> playerNames = new List<string> ();
	public static List<Color> teamColors = new List<Color>();

	void Start(){

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

		for (int i=0; i<3; i++) {
			teamColors.Add(new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f)));
		}

		eventRunner = GameObject.Find ("EventRunner");
	}

	//keep track of player stats OTHER THAN HITS AND DEATHS.
	//TODO: make decisions after collecting the data somewhere. In this class?? TBD.
	void Update(){
	}
	
	public static void CheckPlayerEvents(){

		//check ganging up
		var gangUpStats = PlayerEvents.GetPlayerGangUpStatistics ();


		if (gangUpStats.Count > 1 && gangUpStats.Count < 4 ) {
			//TODO:Respawn players once they die
			TeamUpPlayers(gangUpStats);

		}

		TeamBetrayal ();

		// check team betrayal
	}

	// removes the player from his team
	public static void RemovePlayerFromTeam(string player){
		var team = GetPlayerStats (player).team;
		foreach (var teammate in team) {
			GetPlayerStats(teammate).RemoveTeammate(player);
		}
		GetPlayerStats(player).team = new List<string>();
		GameObject.Find(player).GetComponentsInChildren<ColorSetter>()[0].ResetColor();
	}


	public static void TeamBetrayal(){
		foreach (var playerName in playerNames) {
			var attackers = CheckPlayerGangedUpOn(playerName);
			var playerStats = GetPlayerStats(playerName);

			foreach(var attacker in attackers){
				if(playerStats.isTeammate(attacker)){
					EventController.DisplayMessage(attacker+" betrayed "+playerName,2,new Vector2(0.5f,0.5f),1);
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

	private static void TeamUpPlayers(List<Tuple<string,float>> playerDeathInfos){

		var colorSetters = new List<ColorSetter>();
		foreach(var p in playerDeathInfos){
			var g = GameObject.Find(p.First);
			if(g!=null)
			colorSetters.Add (g.GetComponentsInChildren<ColorSetter>()[0]);
		}

		if(colorSetters.Count != playerDeathInfos.Count) return;

		float teamFormTimeTheshold = 60; //only forms teams of people killed in the last 60 seconds

		// check that the players aren't already in a team


		var nameString = "";
		foreach (var p in playerDeathInfos) {
			var name = p.First;
			var lastDeath = p.Second;
			if(Time.time - teamFormTimeTheshold < lastDeath){
				nameString+=p.First+"\n";
				foreach(var t in playerDeathInfos){
					if(t.First != name){
						var s = GetPlayerStats(name);
						if(s.team.Count != 0) return;
						s.AddTeammate(t.First);
						s.DumpHits();
					}
				}
			}
		}

		var color = teamColors[playerDeathInfos.Count-1];

		foreach (var x in colorSetters){
			x.SetColor(color);
		}

		EventController.DisplayMessage ("A team has been formed!\n" + nameString, 2, new Vector2 (0.5f, 0.5f),0,13);

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

		if (lastHits.Count == 0)
						return new List<string>();

		var attackers = new List<string>();
		/*foreach (var hit in lastHits) {
			var s = attackers.Find( x => x.Equals(hit.attacker));
			if(s == null){
				attackers.Add(hit.attacker);
			}
		}*/

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
		}
		ModifyStat (dead.name, AddDeath, Time.time);
		heatmap.Post (dead.transform.position, deathTag);
	}

	public static void ProdPlayerWithHighestHealth(){
		var playerName = GetPlayerWithHighestHealth ().transform.parent.name;
		EventController.DisplayMessage (playerName + " is doing quite well...", 4, new Vector2 (0.5f, 0.5f));
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
	
	private static PlayerStats GetPlayerStats(string playerName){
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
		//print (p.playerName + " has died at time " + time);
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