using UnityEngine;
using System.Collections;

public class HostileMobMovement : MonoBehaviour {
	
    GameObject player;
    Rigidbody rigidBody;
	EntityController entityController;

	protected void Start () {
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
		entityController = gameObject.GetComponent<EntityController> ();
    }
	
	
	protected void FixedUpdate () {
		if (!entityController.isDead) {
			Vector3 vectorToTarget = player.transform.position - transform.position;
			vectorToTarget.Normalize ();
			rigidBody.MovePosition (transform.position + vectorToTarget * Time.deltaTime * entityController.speed);
			Utils.RotateModel (gameObject, player, 150);
		}
    }


    
}
