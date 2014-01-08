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
	private bool iAmOnTheRedTeam = false;
	private bool iAmOnTheBlueTeam = false;

	// Use this for initialization
	void Start () {
		if (networkView.isMine) {
			myTransform = transform;
			cameraHeadTransform = myTransform.FindChild ("CameraHead");
			GameObject spawnManager = GameObject.Find ("SpawnManager");
			SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
			iAmOnTheBlueTeam = spawnScript.amIOnTheBlueTeam;
			iAmOnTheRedTeam = spawnScript.amIOnTheRedTeam;
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
			string team = iAmOnTheRedTeam ? "red" : "blue";
			networkView.RPC("SpawnProjectile", RPCMode.All, launchPosition, Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90, myTransform.eulerAngles.y, 0), myTransform.name, team);
		}
	}

	[RPC]
	void SpawnProjectile(Vector3 position, Quaternion rotation, string originatorName, string team) {
		GameObject go = Instantiate(blaster, position, rotation) as GameObject;
		BlasterScript blasterScript = go.GetComponent<BlasterScript>();
		blasterScript.myOriginator = originatorName;
		blasterScript.team = team;
	}
}
