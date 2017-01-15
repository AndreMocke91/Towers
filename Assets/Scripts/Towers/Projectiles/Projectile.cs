using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public GameObject target;
	public float speed = 10;
	public float range = 1000;
	public float lifeTimeFactor = 0.008f;
	public bool isHoming = false;
	public float damage = 10f;

	Rigidbody rigidBody;
	public Vector3 targetPosition;

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();

		float lifeTime = range / speed * lifeTimeFactor;
		Invoke ("EndObject", (lifeTime));
	}

	void EndObject()
	{
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {		
		if (target) {				
			if (isHoming) {
				Vector3 vectorToTarget = target.transform.position - transform.position;
				vectorToTarget.Normalize ();
				Utils.RotateModel (this.gameObject, target.gameObject, 100);
				rigidBody.MovePosition (transform.position + vectorToTarget * Time.deltaTime * speed);
			} else {
				if (targetPosition == new Vector3(0,0,0)) {					
					targetPosition = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z);
				}				
				Vector3 vectorToTarget = targetPosition - transform.position;
				vectorToTarget.Normalize ();   
				//Utils.RotateModel (this.gameObject, target.gameObject, 100);
				rigidBody.MovePosition (transform.position + vectorToTarget * Time.deltaTime * speed);
			}
		}
	}

	void OnTriggerEnter(Collider mob)
	{
		//Debug.Log ("Hit mob : " + mob.gameObject.name);
		if (mob.GetComponent<EntityController> () != null && mob.isTrigger && !mob.GetComponent<EntityController>().isDead ) {
			if (mob.transform.gameObject.tag != "Player") {
				mob.GetComponent<EntityController> ().health = mob.GetComponent<EntityController> ().health - damage;
				//Debug.Log ("Mob health is : " + mob.GetComponent<EntityController> ().health);
			}
			Destroy (gameObject);
			return;
		}

		if (mob.GetComponent<ResourceCapsule> () != null && mob.GetComponent<EntityController> () == null)
			Destroy (gameObject);
	}
}
