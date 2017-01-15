using UnityEngine;
using System.Collections;

public class ResourceCapsule : MonoBehaviour {

	public int fruitAmount = 0;
	public int meatAmount = 0;
	public int woodAmount = 0;
	public int stoneAmount = 0;
	public int metalAmount = 0;

	public SkillTreeAbility requiredAbility;

	public void IsDepleted()
	{
		if (fruitAmount <= 0)
		if (meatAmount <= 0)
		if (woodAmount <= 0)
		if (stoneAmount <= 0)
		if (metalAmount <= 0) {
			Destroy(this.gameObject);
		}

	}

}
