using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopUpBuildMenu : MonoBehaviour {

	public List<GameObject> towerComponents;
	public GameObject popupMenuButtonPrefab;

	public void UpdateMenu(GameObject hit)
	{
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (towerComponents.Count, 1);

		for(int i = 0; i < towerComponents.Count; i++)
		{
			GameObject newButton = Instantiate (popupMenuButtonPrefab, popupMenuButtonPrefab.transform.position, Quaternion.identity) as GameObject;
			newButton.GetComponent<Placeable> ().towerComponent = towerComponents [i];
			newButton.GetComponent<Placeable> ().SetComponent (hit);
			newButton.transform.SetParent (transform);
			newButton.GetComponent<RectTransform> ().localPosition = new Vector2 (i * 1.1f, 0);
		}
	}

}
