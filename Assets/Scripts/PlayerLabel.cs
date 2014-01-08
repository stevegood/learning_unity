using UnityEngine;
using System.Collections;

public class PlayerLabel : MonoBehaviour {

	public Texture healthTex;
	public string playerName;

	private Camera myCamera;
	private Transform myTransform;
	private Vector3 worldPosition = new Vector3();
	private Vector3 screenPosition = new Vector3();
	private Vector3 cameraRelativePosition = new Vector3();
	private float minimumZ = 1.5f;
	private int labelTop = 18;
	private int labelWidth = 110;
	private int labelHeight = 15;
	private int barTop = 1;
	private int healthBarHeight = 5;
	private int healthBarLeft = 110;
	private float healthBarLength;
	private float adjustment = 1;
	private GUIStyle myStyle = new GUIStyle();
	private Transform triggerTransform;
	private HealthAndDamage hdScript;

	// Use this for initialization
	void Awake () {
		if (!networkView.isMine) {
			myTransform = transform;
			myCamera = Camera.main;
			myStyle.normal.textColor = (myTransform.tag == "BlueTeam") ? Color.blue : Color.red;
			myStyle.fontSize = 12;
			myStyle.fontStyle = FontStyle.Bold;
			myStyle.clipping = TextClipping.Overflow;
			triggerTransform = myTransform.FindChild("Trigger");
			hdScript = triggerTransform.GetComponent<HealthAndDamage>();
		} else {
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		cameraRelativePosition = myCamera.transform.InverseTransformPoint(myTransform.position);

		if (hdScript.myHealth < 1) {
			healthBarLength = 1;
		} else if (hdScript.myHealth > 0) {
			healthBarLength = (hdScript.myHealth / hdScript.maxHealth) * 100;
		}
	}

	void OnGUI() {
		// only display the player's name if they are in front of the camera
		if (cameraRelativePosition.z > minimumZ) {
			worldPosition = new Vector3(myTransform.position.x, myTransform.position.y + adjustment, myTransform.position.z);
			screenPosition = myCamera.WorldToScreenPoint(worldPosition);

			// draw the health bar
			GUI.Box(new Rect(screenPosition.x - healthBarLeft / 2, Screen.height - screenPosition.y - barTop, 100, healthBarHeight), "");
			GUI.DrawTexture(new Rect(screenPosition.x - healthBarLeft / 2, Screen.height - screenPosition.y - barTop, healthBarLength, healthBarHeight), healthTex);

			// draw the player name label
			GUI.Label(new Rect(screenPosition.x - labelWidth / 2, Screen.height - screenPosition.y - labelTop, labelWidth, labelHeight), playerName, myStyle);
		}
	}
}
