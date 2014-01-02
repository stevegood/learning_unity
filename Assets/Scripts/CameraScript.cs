using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and it
/// causes the camera to follow the CameraHead
/// </summary>

public class CameraScript : MonoBehaviour {

	private Camera myCamera;
	private Transform cameraHeadTransform;

	// Use this for initialization
	void Start () {
		if (networkView.isMine){
			myCamera = Camera.main;
			cameraHeadTransform = transform.FindChild("CameraHead");
		} else {
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// makethe camera follow the player's CameraHead Transform
		myCamera.transform.position = cameraHeadTransform.position;
		myCamera.transform.rotation = cameraHeadTransform.rotation;
	}
}
