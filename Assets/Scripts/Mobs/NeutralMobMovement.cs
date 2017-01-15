using UnityEngine;
using System.Collections;

public class NeutralMobMovement : MonoBehaviour {

    [Range(3,50)]
    public int movementRadius;
    
    public Vector3 destination;
    private Vector3 spawnerPosition;
    private Rigidbody rigidBody;
	private EntityController ec;

	// Use this for initialization
	void Start () {
        spawnerPosition = transform.parent.parent.position;
        rigidBody = GetComponent<Rigidbody>();
		ec = GetComponent<EntityController> ();
       
        InvokeRepeating("RandomizeDestination", 0, 10);
	}	
	
	void FixedUpdate () {
        Vector3 vectorToTarget = destination - transform.position;
        vectorToTarget.Normalize();

        rigidBody.MovePosition(transform.position + vectorToTarget * Time.deltaTime * ec.speed);
    }

    void RandomizeDestination()
    {
        destination = new Vector3(
           Random.Range(spawnerPosition.x - movementRadius, spawnerPosition.x + movementRadius)
           , 0
           , Random.Range(spawnerPosition.z - movementRadius, spawnerPosition.z + movementRadius));
    }


}
