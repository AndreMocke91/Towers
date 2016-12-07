using UnityEngine;
using System.Collections;

public class NeutralMobMovement : MonoBehaviour {

    [Range(3,50)]
    public int movementRadius;
    [Range(0, 5)]
    public float speed;

    public Vector3 destination;
    private Vector3 spawnerPosition;
    private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        spawnerPosition = transform.parent.parent.parent.position;
        rigidBody = GetComponent<Rigidbody>();
       
        InvokeRepeating("RandomizeDestination", 0, 10);
	}	
	
	void FixedUpdate () {
        Vector3 vectorToTarget = destination - transform.position;
        vectorToTarget.Normalize();

        rigidBody.MovePosition(transform.position + vectorToTarget * Time.deltaTime * speed);
    }

    void RandomizeDestination()
    {
        destination = new Vector3(
           Random.Range(spawnerPosition.x - movementRadius, spawnerPosition.x + movementRadius)
           , 0
           , Random.Range(spawnerPosition.z - movementRadius, spawnerPosition.z + movementRadius));
    }


}
