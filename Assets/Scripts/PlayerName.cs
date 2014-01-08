using UnityEngine;
using System.Collections;

public class PlayerName : MonoBehaviour {

	public string playerName;

	void Awake() {
		if (networkView.isMine) {
			playerName = PlayerPrefs.GetString ("playerName");

			foreach (GameObject objNameCheck in GameObject.FindObjectsOfType(typeof(GameObject))) {
				if (playerName == objNameCheck.name) {
					playerName = "(" + Random.Range(0, 1000).ToString() + ")";
					PlayerPrefs.SetString("playerName", playerName);
					break;
				}
			}

			UpdateLocalGameManager(playerName);
			networkView.RPC ("UpdateMyNameEverywhere", RPCMode.AllBuffered, playerName);
		}
	}

	void UpdateLocalGameManager(string pName) {
		GameObject gameManager = GameObject.Find("GameManager");
		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
		dataScript.playerName = pName;
		dataScript.nameSet = true;
	}

	[RPC]
	void UpdateMyNameEverywhere(string pName) {
		gameObject.name = pName;
		playerName = pName;
		PlayerLabel playerLabelScript = transform.GetComponent<PlayerLabel>();
		playerLabelScript.playerName = pName;
	}
}
