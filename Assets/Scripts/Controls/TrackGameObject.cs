using UnityEngine;
using System.Collections;

public class TrackGameObject : MonoBehaviour {

	public GameObject trackedObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = trackedObject.transform.position;
	}
}
