using UnityEngine;
using System.Collections;

/// <summary>
/// Cursor control.
/// Locks and uplocks the cursor when opening the menus
/// </summary>

public class CursorControl : MonoBehaviour {

	private GameObject multiplayerManager;
	private MultiplayerScript multiplayerScript;

	// Use this for initialization
	void Start () {
		if (networkView.isMine) {
			multiplayerManager = GameObject.Find("MultiplayerManager");
			multiplayerScript = multiplayerManager.GetComponent<MultiplayerScript>();
		} else {
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Screen.lockCursor = !multiplayerScript.showDisconnectWindow;
	}
}
