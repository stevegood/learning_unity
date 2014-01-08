﻿using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture crosshairTex;

	private float crosshairDimension = 256;
	private float halfCrosshairWidth = 128;

	// Use this for initialization
	void Start () {
		enabled = networkView.isMine;
	}

	void OnGUI() {
		if (Screen.lockCursor) {
			GUI.DrawTexture(new Rect((Screen.width / 2) - halfCrosshairWidth, (Screen.height / 2) - halfCrosshairWidth, crosshairDimension, crosshairDimension), crosshairTex);
		}
	}
}
