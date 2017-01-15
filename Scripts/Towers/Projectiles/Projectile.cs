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
		Destroy (this.gameObject);
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
					Utils.RotateModel (this.gameObject, target.gameObject, 100);
				}				
				Vector3 vectorToTarget = targetPosition - transform.position;
				vectorToTarget.Normalize ();   
				rigidBody.MovePosition (transform.position + vectorToTarget * Time.deltaTime * speed);
			}
		}
	}

	void OnTriggerEnter(Collider mob)
	{
		//Debug.Log ("Hit mob : " + mob.gameObject.name);
		if (mob.GetComponent<EntityController> () != null) {
			mob.GetComponent<EntityController> ().health = mob.GetComponent<EntityController> ().health - damage;
			Destroy (this.gameObject);
		}
	}
}
