using UnityEngine;
using System.Collections;

public class ColorSetter : MonoBehaviour {
	
	public Color color;
	
	// Use this for initialization
	void Start () {
		//ResetColor ();
		color.a = 1;
	}

	public void SetColor(Color c){
		renderer.material.color = c;
	}

	public void ResetColor(){
		SetColor (color);
	}

}
