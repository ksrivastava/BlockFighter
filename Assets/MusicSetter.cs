using UnityEngine;
using System.Collections;

public class MusicSetter : MonoBehaviour {

	public AudioClip[] clips;

	// Use this for initialization
	void Start () {
		GameObject source = GameObject.FindGameObjectWithTag ("MusicSource");
		MusicPlayer player = source.GetComponent<MusicPlayer> ();
		source.audio.Pause ();
		player.Clips = clips;
	}
}
