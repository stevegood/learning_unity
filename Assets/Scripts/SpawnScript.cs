using UnityEngine;
using System.Collections;

/// <summary>
/// Spawn script.
/// Attached to the spawn manager to allow for player spawns
/// </summary>

public class SpawnScript : MonoBehaviour {

	public bool amIOnTheRedTeam = false;
	public bool amIOnTheBlueTeam = false;
	public Transform redTeamPlayer;
	public Transform blueTeamPlayer;

	private bool justConnectedToServer = false;
	private Rect joinTeamRect;
	private string joinTeamWindowTitle = "Team selection";
	private int joinTeamWindowWidth = 330;
	private int joinTeamWindowHeight = 100;
	private int joinTeamLeftIndent;
	private int joinTeamTopIndent;
	private int buttonHeight = 40;
	private int redTeamGroup = 0;
	private int blueTeamGroup = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (justConnectedToServer) {
			joinTeamLeftIndent = (Screen.width / 2) - (joinTeamWindowWidth / 2);
			joinTeamTopIndent = (Screen.height / 2) - (joinTeamWindowHeight / 2);
			joinTeamRect = new Rect(joinTeamLeftIndent, joinTeamTopIndent, joinTeamWindowWidth, joinTeamWindowHeight);
			joinTeamRect = GUILayout.Window(0, joinTeamRect, JoinTeamWindow, joinTeamWindowTitle);
		}
	}

	void OnConnectedToServer() {
		justConnectedToServer = true;
	}

	void JoinTeamWindow(int windowID) {
		if (GUILayout.Button ("Join RED Team", GUILayout.Height (buttonHeight))) {
			amIOnTheRedTeam = true;
			justConnectedToServer = false;
			SpawnTeamPlayer("SpawnRedTeam", redTeamPlayer, redTeamGroup);
		}

		if (GUILayout.Button ("Join BLUE Team", GUILayout.Height (buttonHeight))) {
			amIOnTheBlueTeam = true;
			justConnectedToServer = false;
			SpawnTeamPlayer("SpawnBlueTeam", blueTeamPlayer, blueTeamGroup);
		}
	}

	void SpawnTeamPlayer(string tag, Transform player, int group) {
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(tag);
		GameObject randomSpawn = spawnPoints[Random.Range (0, spawnPoints.Length)];
		Network.Instantiate(player, randomSpawn.transform.position, randomSpawn.transform.rotation, group);
	}
}
