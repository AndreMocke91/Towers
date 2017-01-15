using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Placeable : MonoBehaviour {

	public GameObject towerComponent;

	GameObject owner;

	void Start()
	{
		//GetComponent<RectTransform> ().localPosition = new Vector3 (0, 0, 0);
	}

	public void SetComponent(GameObject hit)
	{		
		Button btn = GetComponent<Button> ();
		owner = hit;
		btn.onClick.AddListener (delegate {
			PlaceComponent();	
		});

		if (towerComponent.GetComponent<Renderer> () != null)
			GetComponent<RawImage> ().material = towerComponent.GetComponent<Renderer> ().sharedMaterial;

		if (towerComponent.GetComponent<RawImage> () != null)
			GetComponent<RawImage> ().texture = towerComponent.GetComponent<RawImage> ().texture;
	}

	void PlaceComponent()
	{
		Debug.Log ("Placing Component");
		GameObject newTowerComponent = Instantiate (towerComponent, new Vector3(0,0,0), towerComponent.transform.rotation) as GameObject;
		newTowerComponent.transform.parent = owner.transform;
		newTowerComponent.transform.localPosition = towerComponent.transform.localPosition;
		newTowerComponent.transform.localScale = towerComponent.transform.localScale;
		newTowerComponent.transform.localRotation = towerComponent.transform.localRotation;

		if (newTowerComponent.GetComponentInChildren<TargetVector> () != null) {
			newTowerComponent.GetComponentInChildren<TargetVector> ().GetAttributesFromAllComponents ();
		}

		Destroy (this.transform.parent.gameObject);
	}

}
