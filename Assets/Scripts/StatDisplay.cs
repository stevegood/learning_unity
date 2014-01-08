using UnityEngine;
using System.Collections;

public class StatDisplay : MonoBehaviour {

	public Texture healthTex;

	private float health;
	private int healthForDisplay;
	private int boxWidth = 160;
	private int boxHeight = 85;
	private int labelHeight = 20;
	private int labelWidth = 35;
	private int padding = 10;
	private int gap = 120;
	private float healthBarLength;
	private int healthBarHeight = 15;
	private GUIStyle healthStyle = new GUIStyle();
	private HealthAndDamage hdScript;
	private float commonLeft;
	private float commonTop;

	// Use this for initialization
	void Start () {
		if (networkView.isMine) {
			Transform triggerTransform = transform.FindChild("Trigger");
			hdScript = triggerTransform.GetComponent<HealthAndDamage>();
			healthStyle.normal.textColor = Color.green;
			healthStyle.fontStyle = FontStyle.Bold;
		} else {
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		health = hdScript.myHealth;
		healthForDisplay = Mathf.CeilToInt(health);
		healthBarLength = (health / hdScript.maxHealth) * 100;
	}

	void OnGUI() {
		commonLeft = (Screen.width / 2) + 180;
		commonTop = (Screen.height / 2) + 50;
		GUI.Box (new Rect (commonLeft, commonTop, boxWidth, boxHeight), ""); // stats box
		GUI.Box (new Rect (commonLeft + padding, commonTop + padding, 100, healthBarHeight), ""); // box behind the health bar
		GUI.DrawTexture (new Rect (commonLeft + padding, commonTop + padding, healthBarLength, healthBarHeight), healthTex); // draw the health bar
		GUI.Label (new Rect (commonLeft + gap, commonTop + padding, labelWidth, labelHeight), healthForDisplay.ToString(), healthStyle); // show the health
	}
}
