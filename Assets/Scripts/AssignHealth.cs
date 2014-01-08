using UnityEngine;
using System.Collections;

public class AssignHealth : MonoBehaviour {

	void OnConnectedToServer() {
		StartCoroutine(AssignHealthOnJoiningGame());
	}

	IEnumerator AssignHealthOnJoiningGame() {
		// execute after waitTime has ellapsed
		yield return new WaitForSeconds(5);

		// find the trigger game objects of all players and put them in their team array
		assignHealthToTeamPlayers(GameObject.FindGameObjectsWithTag("RedTeamTrigger"));
		assignHealthToTeamPlayers(GameObject.FindGameObjectsWithTag("BlueTeamTrigger"));

		enabled = false;
	}

	void assignHealthToTeamPlayers(GameObject[] team) {
		foreach (GameObject player in team) {
			HealthAndDamage hdScript = player.GetComponent<HealthAndDamage>();
			hdScript.myHealth = hdScript.previousHealth;
		}
	}
}
