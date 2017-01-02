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

	public List<Ability> abilities;
	public List<TowerComponent> towerComponents;
	public List<Edible> edibles;

	public void  CalculateFirstLevelExpRequirements()
	{
		expToNextLevel = CalculateNextLevelExp (currentLevel);
		AdjustSliderRange (expSlider, currentExp, expToNextLevel);
	}

	void AdjustSliderRange(Slider slider, float min, float max)
	{
		slider.minValue = min;
		slider.maxValue = max;
	}

	float CalculateNextLevelExp(float currentLevel)
	{
		return Mathf.Pow (currentLevel, 1.5f) * 15;
	}
}
