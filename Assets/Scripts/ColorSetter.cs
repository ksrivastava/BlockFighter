using UnityEngine;
using System.Collections;

public class ColorSetter : MonoBehaviour {
	
	public Color color;
	
	// Use this for initialization
	void Start () {
		SetColor (color);
	}

	public void SetColor(Color c){
		this.color = c;
		renderer.material.color = color;
	}
	
}
