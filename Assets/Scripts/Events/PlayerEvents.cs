using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEvents : MonoBehaviour {

	static List<PlayerStats> stats;
	static HeatMap heatmap;
	static HeatTag deathTag;
	static string url = "http://iamkos.com/heatmap.php";
	public bool showDeathmap = false;

	
	public static List<string> playerNames = new List<string> ();

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
	}

	//keep track of player stats OTHER THAN HITS AND DEATHS.
	//TODO: make decisions after collecting the data somewhere. In this class?? TBD.
	void Update(){
	}
	
	public static void CheckPlayerEvents(){
		var gangUpStats = PlayerEvents.GetPlayerGangUpStatistics ();
		foreach (var g in gangUpStats) {
			print(g.First+ " was ganged up on!");
		}

		if (gangUpStats.Count == 2) {
			//TODO:Respawn players once they die
			//TeamUpPlayers(gangUpStats);
		}
	}
	
	//returns a list of tuples<playerName,timeOfDeath> if playerName has been ganged up on.
	public static List<Tuple<string,float>> GetPlayerGangUpStatistics(){
		List<Tuple<string,float>> t = new List<Tuple<string,float>>();
		foreach (var playerName in playerNames) {
			if(CheckPlayerGangedUpOn(playerName)){
				t.Add(new Tuple<string,float>(playerName,GetPlayerStats(playerName).GetLastDeath()));
			}
		}
		return t;
	}

	public static void TeamUpPlayers(List<Tuple<string,float>> playerDeathInfos){
		var randomColor = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f)); 
		foreach (var p in playerDeathInfos) {
			var name = p.First;
			var c = GameObject.FindGameObjectWithTag(name).GetComponent<ColorSetter>();
			c.SetColor(randomColor);
		}

	}

	private static bool CheckPlayerGangedUpOn(string playerName){
		var playerStats = GetPlayerStats (playerName);

		if (playerStats.deaths == 0) {
			return false;
		}
		float lookBackDuration = 30f;

		// look back lookBackDuration seconds, see if the player has been hit by >1 other players.

		var lastHits = playerStats.GetLastNSecondsHits (lookBackDuration);

		if (lastHits.Count == 0)
						return false;

		var attackers = new List<string>();
		foreach (var hit in lastHits) {
			var s = attackers.Find( x => x.Contains(hit.attacker));
			if(s == null){
				attackers.Add(hit.attacker);
			}
		}
		return attackers.Count > 1;
	}

	// attacks are hits on players by other players
	public static void RecordAttack(GameObject attackee, GameObject attacker, float damage){
		ModifyStat (attackee.name, AddHit, Time.time, attacker, damage);
	}

	public static void RecordDeath(GameObject dead){
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

	public class PlayerStats {

		public int deaths;
		public List<float> deathTimes;
		public List<hit> hits;
		public string playerName;

		public PlayerStats(string playerName){
			hits = new List<hit>();
			deathTimes = new List<float>();
			this.playerName = playerName;
		}

		public void Hit(string attacker, float damage, float time){
			hit h = new hit ();
			h.attacker = attacker; 
			h.damage = damage;
			h.time = time;
			hits.Add (h);
			//print (attacker + " caused " + damage + " damage to " + this.playerName +" at time " +time);
		}

		public List<hit> GetLastNSecondsHits(float n){
			if (deathTimes.Count == 0)
								return null;
			float lastDeathTime = deathTimes[deathTimes.Count - 1];
			List<hit> h = new List<hit> ();
			for (int i= hits.Count -1; i>=0; --i) {
				if(hits[i].time > lastDeathTime-n){
					h.Add(hits[i]);
				}
			}
			return h;
		}

		public float GetLastDeath(){
			if (deathTimes.Count == 0)
				return float.NaN;
			return deathTimes [deathTimes.Count - 1];
		}

		public class hit{
			public string attacker;
			public float damage;
			public float time;
		}
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

public class Tuple<T1, T2>
{
	public T1 First { get; private set; }
	public T2 Second { get; private set; }
	internal Tuple(T1 first, T2 second)
	{
		First = first;
		Second = second;
	}
}

public static class Tuple
{
	public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
	{
		var tuple = new Tuple<T1, T2>(first, second);
		return tuple;
	}
}
