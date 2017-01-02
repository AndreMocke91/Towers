using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Placeable : MonoBehaviour {

	private GameObject father;
	public GameObject towerComponent;
	private Button button;
	private Camera cam;

	private GameObject tc;
	public bool componentSelected = false;
	ResourceController resourceController;
	ResourceCost resourceCost;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		father = gameObject.transform.parent.gameObject;
		button = GetComponent<Button> ();
		resourceController = GameObject.FindWithTag ("Player").GetComponent<ResourceController> ();
		resourceCost = towerComponent.GetComponent<ResourceCost> ();

		button.onClick.AddListener (() => {
			AttachTowerComponentToPointer(resourceController, resourceCost);
		});

		button.onClick.AddListener (() => {
			CloseFather();
		});
	}

	void CloseFather()
	{
		father.gameObject.SetActive (false);
	}

	void AttachTowerComponentToPointer(ResourceController resourceController, ResourceCost resourceCost)
	{
		if (resourceController.CanAfford (resourceCost)) {
			ComponentPlacer placer = cam.GetComponent<ComponentPlacer> ();
			placer.towerComponent = towerComponent;
			placer.componentSelected = true;
			placer.AttachToPointer ();
		} else {
			Debug.Log ("Insufficient Resources");
		}
	}

}
