using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ComponentPlacer : MonoBehaviour {

	public GameObject towerComponent;
	public bool componentSelected = false;
	public GameObject player;

	public RaycastHit hit; // to see wher the ray lands

	public GameObject tc;
	//bool isInMeleeRange;
	public bool componentPlaced;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && componentSelected)
		{			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (tc != null)				
			if (Physics.Raycast(ray, out hit)){		

				if (hit.collider.gameObject.tag == "BuildableTile" && tc.tag == "TowerBase") {
					SnapBase ();
				}

				if (hit.collider.gameObject.tag == "TowerBase" && (tc.tag == "TowerBody" || tc.tag == "BuildableTile")) {
					SnapBody ();
				}

				if (hit.collider.gameObject.tag == "Projector Slot" && (tc.tag == "TowerProjector" || tc.tag == "BuildableTile")) {
					SnapProjector ();
				}

				if (hit.collider.gameObject.tag == "TowerBase" && tc.tag == "TowerProjectilePile") {
					SnapProjectilePile ();
				}
				
			}

			if (Input.GetKeyUp (KeyCode.R)) {
				Debug.Log ("Rotating component");
				tc.transform.Rotate (new Vector3 (0, 45, 0));
			}

			if (Input.GetMouseButtonUp (0)) {
				if (componentPlaced && tc != null) {
					Debug.Log ("Object Placed, taking pay.");
					GameObject.FindWithTag ("Player").GetComponent<ResourceController> ().DeductFromResources (tc.GetComponent<ResourceCost> ());
					componentPlaced = false;
				}	
				//isInMeleeRange = false;
				componentSelected = false;
				tc = null;
				towerComponent = null;
			} 

			if (Input.GetMouseButtonUp (1)) {
				Destroy (tc);
			}
		}
	}

	public void AttachToPointer()
	{
		componentSelected = true;
		tc = Instantiate (towerComponent, new Vector3(0,0,0), Quaternion.identity) as GameObject;
	}

	void SnapBase()
	{
		Debug.Log ("Found suitable slot for base");
		tc.transform.position = hit.transform.position;
		//isInMeleeRange = player.GetComponent<PlayerController> ().objectsInMeleeRange.Contains (hit.transform.gameObject);
		componentPlaced = true;
	}

	void SnapBody()
	{
		//Debug.Log ("Found suitable slot for Body");
		//isInMeleeRange = player.GetComponent<PlayerController> ().objectsInMeleeRange.Contains (hit.transform.gameObject);
		tc.transform.parent = hit.transform;
		tc.transform.localPosition = towerComponent.transform.localPosition;
		tc.transform.localScale = towerComponent.transform.localScale;
		componentPlaced = true;
	}

	void SnapProjector()
	{
		//Debug.Log ("Found suitable slot for Projector");
		//isInMeleeRange = player.GetComponent<PlayerController> ().objectsInMeleeRange.Contains (hit.transform.gameObject);
		tc.transform.parent = hit.transform;
		tc.transform.localPosition = towerComponent.transform.localPosition;
		tc.transform.localScale = towerComponent.transform.localScale;
		tc.GetComponentInChildren<TargetVector> ().GetAttributesFromAllComponents ();
		componentPlaced = true;
	}

	void SnapProjectilePile()
	{
		Debug.Log ("Found suitable slot for Projectile Pile");
		tc.transform.position = hit.transform.position;
		tc.transform.parent = hit.transform;
		tc.transform.localScale = new Vector3 (1, 1, 1);
		//tc.transform.localPosition = towerComponent.transform.localPosition;
		//tc.GetComponent<Projectile> ().isComponent = true;
		//isInMeleeRange = player.GetComponent<PlayerController> ().objectsInMeleeRange.Contains (hit.transform.gameObject);
		componentPlaced = true;
	}
}
