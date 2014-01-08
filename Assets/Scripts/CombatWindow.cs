using UnityEngine;
using System.Collections;

public class CombatWindow : MonoBehaviour {

	public string attackerName;
	public string destroyedName;
	public bool addNewEntry = false;
	public Rect windowRect;

	private string combatLog;
	private int characterLimit = 10000;
	private int windowLeft = 10;
	private int windowTop = 150;
	private int windowWidth = 300;
	private int windowHeight = 150;
	private GUIStyle myStyle = new GUIStyle();
	private float nextScrollTime = 0;
	private float scrollRate = 12;

	// Use this for initialization
	void Start () {
		myStyle.fontStyle = FontStyle.Bold;
		myStyle.fontSize = 11;
		myStyle.normal.textColor = Color.green;
		myStyle.wordWrap = true;
	}

	void OnGUI() {
		if (Network.peerType != NetworkPeerType.Disconnected) {
			windowTop = Screen.height - 350;
			windowRect = new Rect(windowLeft, windowTop, windowWidth, windowHeight);

			if (addNewEntry) {
				if (combatLog.Length < characterLimit) {
					combatLog = attackerName + " killed " + destroyedName + "\n" + combatLog;
					nextScrollTime = Time.time + scrollRate;
					addNewEntry = false;
				} else {
					// reset the combatLog
					combatLog = attackerName + " killed " + destroyedName;
				}
			}

			windowRect = GUI.Window(4, windowRect, CombatWindowFunction, "Combat Log");

			if (Time.time > nextScrollTime && !addNewEntry) {
				combatLog = "\n" + combatLog;
				nextScrollTime = Time.time + scrollRate;
			}
		}
	}

	void CombatWindowFunction(int windowID) {
		GUILayout.Label(combatLog, myStyle);
	}
}
