using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour {

	public string abilityName;
	public int unlockedAtLevel;
	public bool unlocked;
	public SkillTree parentSkillTree;
	public int rate = 1;

}
