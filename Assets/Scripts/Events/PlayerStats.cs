using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats {
	
	public int deaths;
	public List<float> deathTimes;
	private List<hit> hits;
	public string playerName;
	public List<string> team;
	
	public PlayerStats(string playerName){
		hits = new List<hit>();
		deathTimes = new List<float>();
		this.playerName = playerName;
		this.team = new List<string>();
	}

	public void DumpStats(){
		hits = new List<hit>();
		deathTimes = new List<float>();
		this.team = new List<string>();
	}

	public void DumpHits(){
		hits = new List<hit> ();
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
	
	public void AddTeammate(string teammate){
		if(isTeammate(teammate)) throw new UnityException ("Tried to re add a teammate");
		this.team.Add (teammate);
	}
	
	public void RemoveTeammate(string teammate){
		if (!isTeammate (teammate))
			throw new UnityException ("Tried to remove a non teammate");
		team.Remove (teammate);
		if (team.Count == 0) {
			GameObject.Find(playerName).GetComponentInChildren<ColorSetter>().ResetColor();
		}
	}
	
	public bool isTeammate(string playerName){
		return (this.team.Find (x => x.Equals (playerName)) == playerName);
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
