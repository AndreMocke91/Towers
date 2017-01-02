using UnityEngine;
using System.Collections;

public static class Utils {

	public static void RotateModel(GameObject subject, GameObject target, float speed)
	{	
		Vector3 targetDirection = target.transform.position - subject.transform.position;
		Vector3 newDirection = Vector3.RotateTowards (subject.transform.forward, targetDirection, speed, 0.0f);
		//Debug.DrawRay (subject.transform.position, newDirection, Color.red, 15.0f);
		newDirection.Set (newDirection.x/2, 0, newDirection.z/2);
		subject.transform.rotation = Quaternion.LookRotation (newDirection);
	}

	public static void RotateModel(GameObject subject, Vector3 target, float speed)
	{	
		Vector3 targetDirection = target - subject.transform.position;
		Vector3 newDirection = Vector3.RotateTowards (subject.transform.forward, targetDirection, speed, 0.0f);
		//Debug.DrawRay (subject.transform.position, newDirection, Color.red, 15.0f);
		newDirection.Set (newDirection.x/2, 0, newDirection.z/2);
		subject.transform.rotation = Quaternion.LookRotation (newDirection);
	}

	[System.Serializable]
	public class InspectorChunk{

		public Vector2 origin;
		public GameObject chunk;

		public InspectorChunk(Vector2 _origin, GameObject _chunk)
		{
			origin = _origin;
			chunk = _chunk;
		}
	}
}
