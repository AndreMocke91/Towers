using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

public class ResourceController : MonoBehaviour {
	public int fruitAmount = 0;
	public int meatAmount = 0;
	public int woodAmount = 0;
	public int stoneAmount = 0;
	public int metalAmount = 0;

	public Text fruitAmountText;
	public Text meatAmountText;
	public Text woodAmountText;
	public Text stoneAmountText;
	public Text metalAmountText;

	public int fruitGatherRate = 1;
	public int meatGatherRate = 1;
	public int woodGatherRate = 1;
	public int stoneGatherRate = 1;
	public int metalGatherRate = 1;

	public GameObject skillsGameObject;
	//SkillsController skillsController;

	void Start()
	{
		//skillsController = skillsGameObject.GetComponent<SkillsController> ();
	}

	public void Fruit(ResourceCapsule rc, int gatherRate)
	{
		rc.fruitAmount = rc.fruitAmount - fruitGatherRate;
		fruitAmount = fruitAmount + fruitGatherRate;
		//skillsController.AddGatheringExp (fruitGatherRate);
		UpdateDisplays ();
		rc.IsDepleted ();
	}

	public void Meat(ResourceCapsule rc, int gatherRate)
	{
		rc.meatAmount = rc.meatAmount - gatherRate;
		meatAmount = meatAmount + gatherRate;
		//skillsController.AddHuntingExp (meatGatherRate);
		UpdateDisplays ();
		rc.IsDepleted ();			
	}

	public void Wood(ResourceCapsule rc, int gatherRate)
	{
		rc.woodAmount = rc.woodAmount - gatherRate;
		woodAmount = woodAmount + gatherRate;
		//skillsController.AddGatheringExp (fruitGatherRate);
		UpdateDisplays ();
		rc.IsDepleted ();
	}

	public void Stone(ResourceCapsule rc, int gatherRate)
	{
		rc.stoneAmount = rc.stoneAmount - gatherRate;
		stoneAmount = stoneAmount + gatherRate;
		UpdateDisplays ();

		rc.IsDepleted ();
	}

	public void Metal(ResourceCapsule rc, int gatherRate)
	{
		rc.metalAmount = rc.metalAmount - gatherRate;
		metalAmount = metalAmount + gatherRate;
		UpdateDisplays ();

		rc.IsDepleted ();
	}

	public void Gather(GameObject model, RaycastHit hit, SkillsController skillsController)
	{
		model.GetComponent<Animator> ().SetTrigger ("punching");
		ResourceCapsule rc = hit.transform.gameObject.GetComponent<ResourceCapsule> ();

		if (rc) {	
			Debug.Log ("Attempting Harvest");
			SkillTree playerSkillTree = skillsController.skilltrees.Where (x => x.skillTreeName == rc.requiredAbility.parentSkillTree.skillTreeName && x.abilities.Contains (rc.requiredAbility)).Single ();
			SkillTreeAbility playerAbility = playerSkillTree.abilities.Where (x => x.abilityName == rc.requiredAbility.abilityName).Single ();

			if (playerAbility.unlocked) {
				Debug.Log ("Has sufficient skiiiiil");
				playerSkillTree.AddExp (playerAbility.rate);
				GatherFromCapsule (hit.transform.gameObject.GetComponent<ResourceCapsule> (), playerAbility.rate);
			}
			else {
				Debug.Log ("Cant harvest from this yet... Dummy");
			}
		}
	}

	public bool CanAfford(ResourceCost resourceCost)
	{
		if (resourceCost.fruitAmount <= fruitAmount)
		if (resourceCost.meatAmount <= meatAmount)
		if (resourceCost.woodAmount <= woodAmount)
		if (resourceCost.stoneAmount <= stoneAmount)
		if (resourceCost.metalAmount <= metalAmount) {
			return true;
		}
		return false;				
	}

	public void DeductFromResources(ResourceCost resourceCost)
	{
		fruitAmount = fruitAmount - resourceCost.fruitAmount;
		meatAmount = meatAmount - resourceCost.meatAmount;
		woodAmount = woodAmount - resourceCost.woodAmount;
		stoneAmount = stoneAmount - resourceCost.stoneAmount;
		metalAmount = metalAmount - resourceCost.metalAmount;
		UpdateDisplays ();
	}

	public void UpdateDisplays()
	{
		//fruitAmountText.text = fruitAmount.ToString ();
		meatAmountText.text = meatAmount.ToString ();
		woodAmountText.text = woodAmount.ToString ();
		stoneAmountText.text = stoneAmount.ToString ();
		metalAmountText.text = metalAmount.ToString ();
	}

	public void GatherFromCapsule(ResourceCapsule resourceCapsule, int gatherRate)
	{		
		if (resourceCapsule.fruitAmount > 0)
			Fruit (resourceCapsule, gatherRate);
		else if (resourceCapsule.meatAmount > 0)
			Meat (resourceCapsule, gatherRate);
		else if (resourceCapsule.woodAmount > 0)
			Wood (resourceCapsule, gatherRate);
		else if (resourceCapsule.stoneAmount > 0)
			Stone (resourceCapsule, gatherRate);
		else if (resourceCapsule.metalAmount > 0)
			Metal (resourceCapsule, gatherRate);
	}
		
}
