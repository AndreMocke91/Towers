using UnityEngine;
using System.Collections;

public class BaseEffect : MonoBehaviour {
	
	private PlayerController pc;
	private NeutralMobMovement nmm;
	private HostileMobMovement hmm;

	private bool isPlayer = false;
	// Use this for initialization
	void Start () {

		if ((pc = GetComponent<PlayerController> ()) != null) { // owner is player
			isPlayer = true;
		}
		else if ((nmm = GetComponent<NeutralMobMovement> ()) != null) { //owner is neutral mob
			isPlayer = false;
		}
		else if ((hmm = GetComponent<HostileMobMovement> ()) != null) { //owner is hostile mob
			isPlayer = false;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
