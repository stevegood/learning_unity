using UnityEngine;
using System.Collections;

/// <summary>
/// Fire blaster.
/// This is attached to the player and allows firing of the projectile
/// </summary>

public class FireBlaster : MonoBehaviour {

	public GameObject blaster;
	private Transform myTransform;
	private Transform cameraHeadTransform;
	private Vector3 launchPosition = new Vector3();
	private float fireRate = 0.2f;
	private float nextFire = 0;

	// Use this for initialization
	void Start () {
		if (networkView.isMine) {
			myTransform = transform;
			cameraHeadTransform = myTransform.FindChild ("CameraHead");
		} else {
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("FireWeapon") && Time.time > nextFire && Screen.lockCursor) {
			nextFire = Time.time + fireRate;
			launchPosition = cameraHeadTransform.TransformPoint(0, 0, 0.2f);
			// create the blaster projectile at the launch position

			networkView.RPC("SpawnProjectile", RPCMode.All, launchPosition, Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90, myTransform.eulerAngles.y, 0));
		}
	}

	[RPC]
	void SpawnProjectile(Vector3 position, Quaternion rotation) {
		Instantiate(blaster, position, rotation);
	}
}
