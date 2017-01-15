using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ComponentPlacer : MonoBehaviour {
	
	public RaycastHit hit; // to see wher the ray lands
	public GameObject basePopUpMenu, bodyPopUpMenu, projectorPopUpMenu, projectilePopUpMenu;

	GameObject popupMenu;
	
	// Update is called once per frame
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.anyKey && !EventSystem.current.IsPointerOverGameObject())
			Destroy (popupMenu);

		if (Input.GetMouseButtonUp (1)) {			
			
			if (Physics.Raycast (ray, out hit)) {

				//popup base menu
				if (hit.transform.gameObject.tag == "BuildableTile") {					
					popupMenu = Instantiate (basePopUpMenu, hit.transform.position + basePopUpMenu.transform.position, Quaternion.identity) as GameObject;

				} else if (hit.transform.gameObject.tag == "TowerBase") {
					// check for built components
					bool projectilePilePlaced = false;
					bool bodyPlaced = false;

					foreach (Transform child in hit.transform) {
						if (child.gameObject.tag == "TowerProjectilePile")
							projectilePilePlaced = true;
					}

					foreach (Transform child in hit.transform) {
						if (child.gameObject.tag == "TowerBody")
							bodyPlaced = true;
					}
					//popup body menu
					if (projectilePilePlaced && !bodyPlaced) {
						popupMenu = Instantiate (bodyPopUpMenu, hit.transform.parent.position + bodyPopUpMenu.transform.position, Quaternion.identity) as GameObject;
						//popup projectile menu
					} else if (!projectilePilePlaced)
						popupMenu = Instantiate (projectilePopUpMenu, hit.transform.parent.position + projectilePopUpMenu.transform.position, Quaternion.identity) as GameObject;
					//popup projector menu
				} else if (hit.transform.gameObject.tag == "ProjectorSlot") {
					popupMenu = Instantiate (projectorPopUpMenu, hit.transform.parent.position + projectorPopUpMenu.transform.position, Quaternion.identity) as GameObject;
				// rotate projector on right click -- should change this shit
				} else if (hit.transform.parent.gameObject.tag == "TowerProjector") {
					float angle = hit.transform.parent.parent.GetComponent<TowerBody> ().angle;
					hit.transform.parent.Rotate (new Vector3 (0, angle, 0));
				}

				//adjust menu if suitable location is found
				if (popupMenu != null) {
					Debug.Log ("Opening build menu");
					GameObject baseTile = hit.transform.gameObject;
					while (baseTile.tag.ToString() != "BuildableTile" ) {
						//Debug.Log ((baseTile.tag == "BuildableTile").ToString());
						baseTile = baseTile.transform.parent.gameObject;
					}

					popupMenu.transform.SetParent (baseTile.transform);

					popupMenu.GetComponent<PopUpBuildMenu> ().UpdateMenu (hit.transform.gameObject);
					Utils.RotatePopUpMenu (popupMenu, this.gameObject, 100);
				}
					
					

					
				
			}
		}

	}

}
