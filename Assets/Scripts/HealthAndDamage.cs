using UnityEngine;
using System.Collections;

public class HealthAndDamage : MonoBehaviour {

	public string myAttacker;
	public bool iWasJustAttacked;
	public bool hitByBlaster = false;
	public float myHealth = 100;
	public float maxHealth = 100;
	public float previousHealth = 100;

	private float blasterDamage = 30;
	private GameObject parentObject;
	private bool destroyed = false;
	private float healthRegenRate = 1.3f;

	// Use this for initialization
	void Start () {
		parentObject = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (iWasJustAttacked) {
			GameObject gameManager = GameObject.Find("GameManager");
			PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();

			float lastHealth = myHealth;
			int myPlayerId = int.Parse(Network.player.ToString());
			for (int i = 0; i < dataScript.PlayerList.Count; i++) {
				if (dataScript.PlayerList[i].playerName == myAttacker && myPlayerId == dataScript.PlayerList[i].networkPlayer) {
					if (hitByBlaster && !destroyed) {
						myHealth -= blasterDamage;
					}

					if (myHealth < lastHealth) {
						networkView.RPC ("UpdateMyCurrentAttackerEverywhere", RPCMode.Others, myAttacker);
						networkView.RPC ("UpdateMyCurrentHealthEverywhere", RPCMode.Others, myHealth);
					}
					break;
				}
			}
			iWasJustAttacked = false;
		}

		if (myHealth < 1 && networkView.isMine) {
			GameObject spawnManager = GameObject.Find("SpawnManager");
			SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
			spawnScript.iAmDestroyed = true;

			Network.RemoveRPCs(Network.player);
			networkView.RPC("TellEveryoneWhoDestroyedWho", RPCMode.All, myAttacker, parentObject.name);
			networkView.RPC("DestroySelf", RPCMode.All);
		} else if (myHealth < maxHealth) {
			myHealth += healthRegenRate * Time.deltaTime; // regen health
		} else if (myHealth > maxHealth) {
			myHealth = maxHealth; // don't exceed maxHealth
		}

		if(networkView.isMine && myHealth > 0 && myHealth != previousHealth) {
			networkView.RPC("UpdateMyHealthRecordEverywhere", RPCMode.AllBuffered, myHealth);
		}
	}

	[RPC]
	void UpdateMyCurrentAttackerEverywhere(string attacker) {
		myAttacker = attacker;
	}

	[RPC]
	void UpdateMyCurrentHealthEverywhere(float health) {
		myHealth = health;
	}

	[RPC]
	void DestroySelf() {
		Destroy (parentObject);
	}

	[RPC]
	void UpdateMyHealthRecordEverywhere(float health) {
		previousHealth = health;
	}

	[RPC]
	void TellEveryoneWhoDestroyedWho(string _attacker, string _destroyed) {
		GameObject gameManager = GameObject.Find ("GameManager");
		CombatWindow combatScript = gameManager.GetComponent<CombatWindow>();
		combatScript.attackerName = _attacker;
		combatScript.destroyedName = _destroyed;
		combatScript.addNewEntry = true;
	}
}
