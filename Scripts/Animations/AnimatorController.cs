using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour {

	public bool walking = false;

	void Start()
	{
		
	}

	public void RotateModel(GameObject subject, GameObject target, float speed)
	{	
		Vector3 targetDirection = target.transform.position - subject.transform.position;
		Vector3 newDirection = Vector3.RotateTowards (subject.transform.forward, targetDirection, speed, 0.0f);
		Debug.DrawRay (subject.transform.position, newDirection, Color.red, 15.0f);
		newDirection.Set (newDirection.x/2, 0, newDirection.z/2);
		if(newDirection != Vector3.zero)
		subject.transform.rotation = Quaternion.LookRotation (newDirection);
	}
}
