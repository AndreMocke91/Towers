using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {

	public float neutralWaitTime, neutralWaveInterval;
	public float hostileWaitTime, hostileWaveInterval;

	public List<NeutralMobSpawner> neutralMobSpawners;
	public GameObject[] hostileMobs;
	public List<GameObject> activeHostileMobs;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("SpawnWave", hostileWaitTime, hostileWaveInterval);
		InvokeRepeating ("ReplenishNeutrals", neutralWaitTime, neutralWaveInterval);
	}

	public void SpawnWave()
	{
		foreach (GameObject hm in activeHostileMobs) {
			Destroy (hm);
		}
		Debug.Log ("Spawning wave...");
		foreach (NeutralMobSpawner ns in neutralMobSpawners) {
			activeHostileMobs.Add (Instantiate (hostileMobs [0], ns.transform.position, Quaternion.identity) as GameObject);
		}
	}

	public void ReplenishNeutrals()
	{
		Debug.Log ("Replenishing Neutrals");
		foreach (NeutralMobSpawner ns in neutralMobSpawners) {
			if (!ns.HasMob ())
				ns.SpawnMob ();
		}
	}
}
