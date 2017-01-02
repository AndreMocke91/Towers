using UnityEngine;
using UnityEngine.EventSystems;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


public class PlayerController : MonoBehaviour {

    public Camera mainCamera;
	public List<GameObject> objectsInMeleeRange;
	public GameObject skillsHUD;
	public SkillsController skillsController;
   
    Rigidbody rigidBody;
	EntityController ec;
	GameObject model;
	ResourceController resourceController;
	float timeSinceLastGather;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
		ec = GetComponent<EntityController> ();
		model = GetComponentInChildren<AnimatorController> ().gameObject as GameObject;
		objectsInMeleeRange = new List<GameObject> ();
		resourceController = GetComponent<ResourceController> ();
		//skills.GetComponent<SkillsController> ().ApplyFirstExpFunctions ();

    }
	
	// Update is called once per frame
	void Update () {
		timeSinceLastGather += Time.deltaTime;
		if (ShouldMove ()) {
			RaycastHit hit;
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				
				model.GetComponent<AnimatorController> ().RotateModel (model, hit.transform.gameObject, 150); //rotate to face towards pointer

				if (objectsInMeleeRange.Contains (hit.transform.gameObject)) {	// if close enough to object, attempt to gather				
					if (timeSinceLastGather >= 0.5f) { // this check allows to gather once every second
						resourceController.Gather (model, hit, skillsController);			
						timeSinceLastGather = 0;
					}
				} else { // else walk to object
					Walk (hit);
				}
			}
		} else
			model.GetComponent<Animator> ().SetBool ("walking", false); // else do nothing	

		if (Input.GetKeyUp (KeyCode.K))
			skillsHUD.SetActive (!skillsHUD.activeInHierarchy);
	}

	void OnTriggerEnter(Collider col)
	{
		objectsInMeleeRange.Add (col.transform.gameObject);
	}

	void OnTriggerExit(Collider col)
	{
		objectsInMeleeRange.Remove (col.transform.gameObject);
	}
    
	bool ShouldMove()
	{
		if (Input.GetMouseButton(0))
		if(!EventSystem.current.IsPointerOverGameObject())
		//if(!mainCamera.GetComponent<ComponentPlacer>().componentSelected)			
			return true;
		return false;			
	}

	void Walk(RaycastHit hit)
	{
		Transform objectHit = hit.transform;
		Vector3 vectorToTarget = objectHit.position - transform.position;
		vectorToTarget.Normalize ();                   

		rigidBody.MovePosition (transform.position + vectorToTarget * Time.deltaTime * ec.speed);
		model.GetComponent<Animator> ().SetBool ("walking", true);
	}

}
