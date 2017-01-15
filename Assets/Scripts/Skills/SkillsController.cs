using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillsController : MonoBehaviour {

	public GameObject skillsUI;
	public GameObject skillTreePrefab;
	public List<SkillTree> skilltrees;

	public List<GameObject> skillCanvases;

	public void UpdateSkillTreeUI()
	{
		skillCanvases = new List<GameObject>(); 
		for (int i = 0; i < skilltrees.Count; i++) {
			GameObject newTab = Instantiate (skillTreePrefab, Vector3.one, Quaternion.identity) as GameObject;

			newTab.transform.SetParent (skillsUI.transform);

			newTab.GetComponent<RectTransform> ().localPosition = skillTreePrefab.GetComponent<RectTransform> ().localPosition;
			Vector3 newPos = newTab.GetComponent<RectTransform> ().localPosition;
			newTab.GetComponent<RectTransform> ().localPosition = newPos + new Vector3 (i * 100, 0, 0);

			newTab.GetComponentInChildren<Text> ().text = skilltrees [i].skillTreeName;
			GameObject skillsCanvas = newTab.transform.FindChild ("Skills Canvas").gameObject;

			if (i > 0)
				skillsCanvas.SetActive (false);
			
			skillCanvases.Add (skillsCanvas);
			skilltrees [i].skillCanvas = skillsCanvas;
			skilltrees [i].LinkCanvas ();

			skillsCanvas.GetComponent<RectTransform>().localPosition = skillsCanvas.GetComponent<RectTransform>().localPosition - new Vector3 (i * 100 + 5 * i + i, 0, 0);

			newTab.GetComponent<Button>().onClick.AddListener (
				delegate{OnSkillTabClick( skillsCanvas );}
			);
		}
	}

	void OnSkillTabClick(GameObject skillsCanvas)
	{
		foreach (GameObject go in skillCanvases)
			go.SetActive (false);
		skillsCanvas.SetActive (true);
	}

}
