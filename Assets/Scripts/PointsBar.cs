﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsBar : MonoBehaviour {

	public float x, y;
	public float length, height;
	public GUIStyle[] styles;
	
	public Texture2D p1Texture;
	public Texture2D p2Texture;
	public Texture2D p3Texture;
	public Texture2D p4Texture;
	
	private Dictionary<string, int> points;
	private float total = 4.0f;

	private Rect barRect;

	void Start() {
		styles = new GUIStyle[4];
		points = new Dictionary<string, int> ();
		var pl = GameObject.FindGameObjectsWithTag("Player");
		foreach (var p in pl) {
			print (p.transform.parent.name);
			points.Add(p.transform.parent.name, 1);
		}

		for (int i = 0; i < styles.Length; ++i) {
			styles[i] = new GUIStyle();
		}

		styles[0].normal.background = p1Texture;
		styles[1].normal.background = p2Texture;
		styles[2].normal.background = p3Texture;
		styles[3].normal.background = p4Texture;
	}

	void OnGUI() {
		barRect = new Rect (x, y, length, height);
		GUI.Box (barRect, "");

		int i = 0;
		float last = 0;
		foreach (int value in points.Values) {
			GUI.Box (new Rect(barRect.xMin + last * length, y, length * (value / total), height), "", styles[i++]);
			last += (value / total);
		}
	}

	public void AddPoints(string objName, int p) {
		points[objName] += p;
		total += p;
	}

}
