using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetVector : MonoBehaviour {

	public float maxRange = 1000;
	public float maxAngle = 30;
	public float maxHeight = 10;
	public bool visible = true;
	public int numberOfShots = 1;
	public float refireRate = 1f;

	public List<EntityController> hostileMobsInRange;
	public GameObject target;
	public List<GameObject> projectiles;

	public Effector towerBase;
	public TowerBody towerBody;
	public GameObject towerProjectile;
	public GameObject rotationalPart;
	// Use this for initialization

	Effector baseEffector;
	void Start () {	
		InvokeRepeating (
			"Shoot",
			2f,
			refireRate);

		//this.transform.position = this.transform.position - new Vector3 (0, 2.5f, 0);
	}

	void FixedUpdate()
	{		
		if (hostileMobsInRange != null && hostileMobsInRange.Count != 0) {
			if (hostileMobsInRange [0] != null) {
				target = hostileMobsInRange [0].gameObject;
				Utils.RotateModel (rotationalPart, target, 100);
			} else
				hostileMobsInRange.RemoveAt (0);
		}
		else
			target = null;
			
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
	{
		Vector3 dir = point - pivot;
		dir = Quaternion.Euler (angle) * dir;
		point = dir + pivot;
		return point;
	}

	void OnTriggerStay (Collider mob)
	{	
		if (mob.gameObject.GetComponent<EntityController> () != null && mob.gameObject.GetComponent<EntityController> ().health <= 0)
			hostileMobsInRange.Remove (mob.gameObject.GetComponent<EntityController> ());	 
	}

	void OnTriggerEnter(Collider mob)
	{
		if (mob.GetComponent<EntityController> () != null && mob.GetComponent<EntityController> ().isHostile && !mob.isTrigger )
			hostileMobsInRange.Add (mob.GetComponent<EntityController> ());
	}

	void OnTriggerExit(Collider mob)
	{
		if (mob.GetComponent<EntityController> () != null && mob.GetComponent<EntityController> ().isHostile)
			hostileMobsInRange.Remove (mob.GetComponent<EntityController> ());

		if (mob.gameObject.tag == "TowerProjectile") {
			float distance = Vector3.Distance (this.gameObject.transform.position, mob.gameObject.transform.position);
			if (distance > maxRange)
				Destroy (mob.gameObject);
		}
	}

	Vector3[] AssignVerticesForWedge()
	{
		Vector3 heightVector = new Vector3 (0, maxHeight * 0.8f, 0);

		return new Vector3[] { 
			new Vector3 (0, 0, 0) - heightVector, //0
			RotatePointAroundPivot (new Vector3 (0, 0, maxRange), new Vector3 (0, 0, 0), new Vector3 (0, maxAngle/2, 0)) - heightVector, //1
			RotatePointAroundPivot (new Vector3 (0, 0, maxRange), new Vector3 (0, 0, 0), new Vector3 (0, -maxAngle/2, 0)) - heightVector, //2

			new Vector3 (0, 0, 0) + heightVector, //3
			RotatePointAroundPivot (new Vector3 (0, 0, maxRange), new Vector3 (0, 0, 0), new Vector3 (0, maxAngle/2, 0)) + heightVector, //4
			RotatePointAroundPivot (new Vector3 (0, 0, maxRange), new Vector3 (0, 0, 0), new Vector3 (0, -maxAngle/2, 0)) + heightVector //5
		};
	}

	int[] AssignTrianglesForWedge()
	{
		return new int[]
		{ 	0, 1, 2, //bottom face
			3, 4, 1, //side face
			3, 1, 0, //side face
			4, 5, 2, //front face
			4, 2, 1, //front face
			5, 3, 2, //side face
			2, 3, 0, //side face
			5, 4, 3 // top face
		};
	}

	public void GetAttributesFromAllComponents()
	{
		towerBody = this.transform.parent.parent.parent.gameObject.GetComponentInChildren<TowerBody> ();
		towerBase = this.transform.parent.parent.parent.parent.gameObject.GetComponentInChildren<Effector> ();
		towerProjectile = this.transform.parent.parent.parent.parent.gameObject.GetComponentInChildren<ProjectilePile> ().projectile;
		baseEffector = towerBase.GetComponent<Effector> ();

		maxAngle = towerBody.angle;
		maxRange = towerBody.range;

		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();

		mesh.vertices = AssignVerticesForWedge ();
		mesh.triangles = AssignTrianglesForWedge ();
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds();

		GetComponent<MeshCollider> ().sharedMesh = mesh;
	}

	void Shoot()
	{		
		if (target != null && towerProjectile != null) {		
			Debug.Log ("shooting");	
			GameObject projectile = Instantiate (towerProjectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;

			projectile.transform.parent = this.gameObject.transform;
			projectile.transform.localScale = new Vector3 (10, 5, 100);
			projectile.GetComponent<Projectile> ().target = target;
			projectile.GetComponent<Projectile> ().range = towerBody.range;

			Effector effector = projectile.AddComponent<Effector>() as Effector;
			effector.slowDuration = baseEffector.slowDuration;
			effector.slowPercentage = baseEffector.slowPercentage;
		}
	}

}
