using UnityEngine;
using System.Collections;

public class SkillTreeTowerComponent : MonoBehaviour {
	
	public string towerComponentName;
	public int unlockedAtLevel;
	public bool unlocked;
	public SkillTree parentSkillTree;
	public GameObject towerComponent;
}
