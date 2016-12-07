using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeutralMobSpawner : MonoBehaviour {

    public GameObject neutralMob;
    [Range(0,10)]
    public int numberOfMobs;
	public int mobXRotation;

    public float spawnWait;

    private List<GameObject> mobs = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
		Quaternion rot = Quaternion.Euler(mobXRotation, 0, 0);

        GameObject mob = Instantiate(neutralMob, pos, rot) as GameObject;
        mob.transform.parent = transform;

        mobs.Add(mob);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
