using UnityEngine;
using System.Collections;

/// <summary>
/// Blaster script.
/// attached to the Blaster projectile and governs its behavior
/// </summary>

public class BlasterScript : MonoBehaviour {

	public GameObject blasterExplosion;
	public string team;
	public string myOriginator;

	private Transform myTransform;
	private float projectileSpeed = 10;
	private bool expended = false;
	private RaycastHit hit;
	private float range = 1.5f;
	private float expireTime = 5;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		// start the expire timer
		StartCoroutine(DestroyMyselfAfterSomeTime ());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);
		if (Physics.Raycast(myTransform.position, myTransform.up, out hit, range) && !expended) {
			if (hit.transform.tag == "Floor") {
				expended = true;
				Instantiate(blasterExplosion, hit.point, Quaternion.identity);
				myTransform.renderer.enabled = false;
				myTransform.light.enabled = false;
			} else if (hit.transform.tag == "BlueTeamTrigger" || hit.transform.tag == "RedTeamTrigger") {
				expended = true;
				Instantiate(blasterExplosion, hit.point, Quaternion.identity);
				myTransform.renderer.enabled = false;
				myTransform.light.enabled = false;
				
				if ((hit.transform.tag == "BlueTeamTrigger" && team == "red") || (hit.transform.tag == "RedTeamTrigger" && team == "blue")) {
					HealthAndDamage hdScript = hit.transform.GetComponent<HealthAndDamage>();
					hdScript.iWasJustAttacked = true;
					hdScript.myAttacker = myOriginator;
					hdScript.hitByBlaster = true;
				}
			}
		}
	}

	IEnumerator DestroyMyselfAfterSomeTime(){
		// wait for the timer to count up to the expire time
		// then destroy the projectile
		yield return new WaitForSeconds(expireTime);

		Destroy(myTransform.gameObject);
	}
}
