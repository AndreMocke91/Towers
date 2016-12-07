using UnityEngine;
using System.Collections;

public class HostileMobMovement : MonoBehaviour {

    [Range(0, 5)]
    public float speed;

    private GameObject player;
    private Rigidbody rigidBody;

    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	
	void FixedUpdate () {
        Vector3 vectorToTarget = player.transform.position - transform.position;
        vectorToTarget.Normalize();

        rigidBody.MovePosition(transform.position + vectorToTarget * Time.deltaTime * speed);
    }
    
}
