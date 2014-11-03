using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEvents : MonoBehaviour {

	static List<PlayerStats> stats;
	static HeatMap heatmap;
	static HeatTag deathTag;
	static string url = "http://iamkos.com/heatmap.php";
	public bool showDeathmap = false;


	void Start(){
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

	// attacks are hits on players by other players
	public static void RecordAttack(GameObject attackee, GameObject attacker, float damage){
		ModifyStat (attackee.name, AddHit, attackee, attacker, damage);
	}

	public static void RecordDeath(GameObject dead){
		ModifyStat (dead.name, AddDeath);
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
		List<hit> hits;
		public string playerName;

		public PlayerStats(string playerName){
			hits = new List<hit>();
			this.playerName = playerName;
		}

		public void Hit(string attacker, string attackee, float damage){
			hit h = new hit ();
			h.attackee = attackee; 
			h.attacker = attacker; 
			h.damage = damage;
			hits.Add (h);

			//print (attacker + " caused " + damage + " damage to " + attackee);
		}

		public class hit{
			public string attacker,attackee;
			public float damage;
		}
	}

	// helper stuff for tracking stats
	public delegate void Func(params object[] values);
	
	private static void ModifyStat(string playerName, Func f, params object[] values){
		foreach (var s in stats) {
			if(s.playerName == playerName){
				f(s,values);
				return;
			}
		}
	}

	private static void AddDeath(params object[] values){
		PlayerStats p = values [0] as PlayerStats;
		p.deaths++;
		//print (p.playerName + " has died!");
	}

	private static void AddHit(params object[] values){
		PlayerStats p = values [0] as PlayerStats;
		var valuesTwo = values [1] as object[];
		var attackee = valuesTwo[0] as GameObject;
		var attacker = valuesTwo[1] as GameObject;
		var damage = (float)valuesTwo[2];
		p.Hit (attacker.name, attackee.name, damage);
	}
}
