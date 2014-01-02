using UnityEngine;
using System.Collections;

/// <summary>
/// Movement update.
/// Ensures that every player's position, rotation and scale are
/// kept synchronized
/// </summary>

public class MovementUpdate : MonoBehaviour {

	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Transform myTransform;

	// Use this for initialization
	void Start () {
		if (networkView.isMine) {
			myTransform = transform;
			networkView.RPC("UpdateMovement", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
		} else {
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool hasMoved = Vector3.Distance (myTransform.position, lastPosition) > 0;
		bool hasTurned = Quaternion.Angle (myTransform.rotation, lastRotation) > 0;
		if (hasMoved || hasTurned) {
			lastPosition = myTransform.position;
			lastRotation = myTransform.rotation;
			networkView.RPC("UpdateMovement", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
		}
	}

	[RPC]
	void UpdateMovement(Vector3 newPosition, Quaternion newRotation) {
		transform.position = newPosition;
		transform.rotation = newRotation;
	}
}
