using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointLights : MonoBehaviour ,IEvent{

	public float eventDuration = 25;


	private float zDisplacement = -10f;
	private float yDisplacement = -1f;
	private float intensityDelta = 0.05f;
	private float intensityTimeDelta = 0.05f;
	private float finalIntensity = 0.8f;
	private GameObject dirLight;

	private List<GameObject> lights = new List<GameObject>();

	public void Begin(){
		//print ("PointLights begin " + Time.time);
		EventController.DisplayMessage("Get ready for the darkness!",2,new Vector2(0.5f,0.5f));
		dirLight = GameObject.Find ("Directional light") as GameObject;
		FadeOut ();
		var players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (var player in players) {
			var pointLight = Instantiate (Resources.Load ("PointLight")) as GameObject;
			pointLight.transform.parent = player.gameObject.transform;
			pointLight.SetActive(true);
			var pos = player.transform.position;
			pos.z = zDisplacement;
			pos.y = yDisplacement;
			pointLight.transform.position = pos;
			lights.Add(pointLight);
		}

		Invoke ("End", eventDuration);	
	}
	
	public virtual void End(){
		FadeIn ();
	}
	
	private void FadeOut(){

		var intensity = dirLight.light.intensity;
		var t = 0f;
		while (intensity >= 0) {
			Invoke ("SetColourDown", t);
			t += intensityTimeDelta;
			intensity -= intensityDelta;
		}
	}

	private void FadeIn(){
		var intensity = dirLight.light.intensity;
		var t = 0f;
		while (intensity < finalIntensity ) {
			Invoke ("SetColourUp", t);
			t += intensityTimeDelta;
			intensity += intensityDelta;
		}
	}

	private void SetColourDown(){
		if( dirLight.light.intensity > 0)
		dirLight.light.intensity = dirLight.light.intensity - intensityDelta;
	}

	private void SetColourUp(){

		dirLight.light.intensity = dirLight.light.intensity + intensityDelta;
		if (dirLight.light.intensity >= finalIntensity)
						CleanUp ();
	}

	private void CleanUp(){
		EventController.EventEnd (EventType.PointLights, EventType.Idle, 1);
		foreach (var light in lights) {
			Destroy(light);
		}
		//print ("PointLights End " + Time.time);
		Destroy (this.gameObject);
	}

	public void OnDestroy(){}

}
