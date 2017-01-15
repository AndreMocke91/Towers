using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillTree : MonoBehaviour {

	public string skillTreeName;
	public Text levelText;
	public Slider expSlider;
	public int currentLevel = 1;
	public float currentExp = 0;
	public float expToNextLevel;
	public GameObject skillCanvas;
	public GameObject skillIconPrefab;

	public List<SkillTreeAbility> abilities;
	public List<SkillTreeTowerComponent> towerComponents;
	public List<SkillTreeEdible> edibles;

	public void  CalculateFirstLevelExpRequirements()
	{
		CalculateNextLevelExp ();
		AdjustSliderRange ();
		AdjustLevelTexts ();
	}

	void AdjustSliderRange()
	{
		expSlider.minValue = currentExp;
		expSlider.maxValue = expToNextLevel;
	}

	void CalculateNextLevelExp()
	{
		expToNextLevel = Mathf.Pow (currentLevel, 1.5f) * 15;
	}

	public void LinkCanvas()
	{
		expSlider = skillCanvas.GetComponentInChildren<Slider> ();
		CalculateFirstLevelExpRequirements ();
		UpdateCanvas ();
	}

	public void AdjustLevelTexts()
	{
		skillCanvas.transform.FindChild ("Current Level").GetComponent<Text> ().text = currentLevel.ToString();
		skillCanvas.transform.FindChild ("Next Level").GetComponent<Text> ().text = (currentLevel + 1).ToString();
	}

	public void AddExp(float expAmount)
	{
		currentExp += expAmount;
		expSlider.value = currentExp;

		if (currentExp >= expToNextLevel) {
			currentLevel++;
			CalculateNextLevelExp ();
			AdjustSliderRange ();
			AdjustLevelTexts ();

		}
	}

	public void UpdateCanvas()
	{
		for (int i = 0; i < towerComponents.Count; i++) {
			//Debug.Log (towerComponents [i].gameObject.name);
			GameObject newSkillIcon = Instantiate (skillIconPrefab, skillIconPrefab.GetComponent<RectTransform>().transform.position, Quaternion.identity) as GameObject;
			newSkillIcon.transform.SetParent (skillCanvas.transform);
			newSkillIcon.GetComponent<RawImage> ().texture = towerComponents [i].towerComponent.GetComponent<RawImage> ().texture;

			newSkillIcon.GetComponent<RectTransform> ().localPosition = new Vector2 (-350 + 36 * towerComponents[i].unlockedAtLevel, +150);
		}
	}
}
