using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerComponentSelectorMenu : MonoBehaviour {

	public GameObject[] towerComponents;
	public Button baseButton;

	private RectTransform border;
	// Use this for initialization
	void Start () {

		Button btn;
		float width = baseButton.GetComponent<RectTransform> ().rect.width;

		for (int i = 0; i < towerComponents.Length; i++) {
			btn = Instantiate (baseButton, baseButton.transform.position, baseButton.transform.rotation) as Button;
			btn.transform.SetParent (this.transform);
			btn.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
			btn.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
			btn.transform.localPosition = new Vector3 (36 + (width*i), -30, 0);
			btn.transform.localScale = new Vector3 (1, 1, 1);

			btn.GetComponent<Placeable> ().towerComponent = towerComponents [i];
			btn.GetComponentInChildren<Text> ().text = towerComponents [i].name;
		}
	}

}
