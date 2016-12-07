using UnityEngine;
using System.Collections;

public class TowerBase : MonoBehaviour {
	
	[Range(0f,100f)]
	public float slow;
	public float damage;

	private float initialSpeed = 0;
	private PlayerController pc;
	private NeutralMobMovement nmm;

	void OnTriggerEnter(Collider mob)
	{		
		if (slow > 0f) {
			ApplySlow (mob);
		}

		if (damage > 0f) {
			ApplyDamage (mob);
		}
	}

	void OnTriggerExit(Collider mob)
	{
		if (slow > 0) {
			if (mob.transform.parent.GetComponent<PlayerController> () != null) { //player
				pc.speed = initialSpeed;
			} else if (mob.GetComponent<NeutralMobMovement> () != null) { //neutral mob
				nmm.speed = initialSpeed;
			} else {
				return;
			}
		}
	}

	void ApplySlow(Collider mob)
	{
		Debug.Log ("Slowing mob:");

		if (mob.transform.parent.GetComponent<PlayerController> () != null) { //player
			pc = mob.transform.parent.GetComponent<PlayerController> ();
			if (initialSpeed == 0f)
				initialSpeed = pc.speed;
			pc.speed = initialSpeed * (slow / 100f);
		} else if (mob.GetComponent<NeutralMobMovement> () != null) { //neutral mob
			nmm = mob.GetComponent<NeutralMobMovement> ();
			if (initialSpeed == 0f)
				initialSpeed = nmm.speed;
			nmm.speed = initialSpeed * (slow / 100f);
		} else {
			return;
		}
	}

	void ApplyDamage(Collider mob)
	{
		if (mob.transform.parent.GetComponent<EntityController> () != null) { //player
			EntityController ec = mob.transform.parent.GetComponent<EntityController> ();
			ec.ApplyDamage (damage);
		}
	}
}
