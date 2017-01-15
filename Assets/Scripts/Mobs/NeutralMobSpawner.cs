using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeutralMobSpawner : MonoBehaviour {

    public GameObject neutralMob;    
	public int mobXRotation;
    public float spawnWait;    

	// Use this for initialization
	void Start () {		
		SpawnMob ();
    }

	public void SpawnMob()
	{
		//Debug.Log ("Spawning mob for " + this.gameObject.name);
		Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
		Quaternion rot = Quaternion.Euler(mobXRotation, 0, 0);

		GameObject mob = Instantiate(neutralMob, pos, rot) as GameObject;
		mob.transform.parent = transform;
	}

	public bool HasMob()
	{
		if (this.gameObject.GetComponentInChildren<EntityController> () != null) {			
			return true;
		}
		return false;
	}
}
