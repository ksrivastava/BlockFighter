using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	
	private int curAudio = 0;	
	public AudioClip[] clips;
	public AudioClip[] Clips {
		get { return clips; }
		set { 
			clips = value;
			curAudio = 0;
			gameObject.audio.loop = false;
		}
	}

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		if (GameObject.FindGameObjectsWithTag ("MusicSource").Length > 1) {
			Destroy(gameObject);
		}
		else {
			gameObject.audio.enabled = true;
		}
	}

	void Update() {	
		if(!gameObject.audio.isPlaying) {
			if(curAudio > clips.Length - 1) {
				curAudio = clips.Length - 1;
			}		
			gameObject.audio.clip = clips[curAudio];
			if (curAudio == clips.Length - 1) {
				gameObject.audio.loop = true;
			}
			gameObject.audio.Play();
			curAudio++;
		}
	}
}
