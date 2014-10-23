using UnityEngine;
using System.Collections;

public class ColorSetter : MonoBehaviour {
	
	public Color color;
	
	// Use this for initialization
	void Start () {
		renderer.material.color = color;
	}
	
}
