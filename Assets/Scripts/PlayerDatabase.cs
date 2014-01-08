using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Player database.
/// this script manages the player list and is attached to the GameManager
/// </summary>

public class PlayerDatabase : MonoBehaviour {

	public List<PlayerDataClass> PlayerList = new List<PlayerDataClass>();
	public NetworkPlayer networkPlayer;
	public bool nameSet = false;
	public string playerName;
	public bool scored = false;
	public int playerScore;
	public bool joinedTeam = false;
	public string playerTeam;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (nameSet) {
			networkView.RPC ("EditPlayerListWithName", RPCMode.AllBuffered, Network.player, playerName);
			nameSet = false;
		}

		if (scored) {
			networkView.RPC ("EditPlayerWithScore", RPCMode.AllBuffered, Network.player, playerScore);
			scored = false;
		}

		if (joinedTeam) {
			networkView.RPC ("EditPlayerWithTeam", RPCMode.AllBuffered, Network.player, playerTeam);
			joinedTeam = false;
		}
	}

	void OnPlayerConnected(NetworkPlayer netPlayer) {
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, netPlayer);
	}

	void OnPlayerDisconnected(NetworkPlayer netPlayer) {
		networkView.RPC ("RemovePlayerFromList", RPCMode.AllBuffered, netPlayer);
	}

	PlayerDataClass FindPlayerDataByNetworkPlayer(NetworkPlayer netPlayer) {
		int netPlayerID = int.Parse (netPlayer.ToString ());
		for (int i=0; i < PlayerList.Count; i++) {
			if (PlayerList[i].networkPlayer == netPlayerID) {
				return PlayerList[i];
			}
		}
		return null;
	}

	[RPC]
	void AddPlayerToList(NetworkPlayer netPlayer) {
		PlayerDataClass capture = new PlayerDataClass();
		capture.networkPlayer = int.Parse(netPlayer.ToString());
		PlayerList.Add(capture);
	}

	[RPC]
	void RemovePlayerFromList(NetworkPlayer netPlayer) {
		int netPlayerID = int.Parse (netPlayer.ToString ());
		int playerIndex = -1;
		for (int i=0; i < PlayerList.Count; i++) {
			if (PlayerList[i].networkPlayer == netPlayerID) {
				playerIndex = i;
				break;
			}
		}

		if (playerIndex >= 0) {
			PlayerList.RemoveAt(playerIndex);
		}
	}

	[RPC]
	void EditPlayerListWithName(NetworkPlayer netPlayer, string pName) {
		FindPlayerDataByNetworkPlayer (netPlayer).playerName = pName;
	}

	[RPC]
	void EditPlayerListWithScore(NetworkPlayer netPlayer, int score) {
		FindPlayerDataByNetworkPlayer (netPlayer).playerScore = score;
	}

	[RPC]
	void EditPlayerListWithTeam(NetworkPlayer netPlayer, string team) {
		FindPlayerDataByNetworkPlayer (netPlayer).playerTeam = team;
	}
}
