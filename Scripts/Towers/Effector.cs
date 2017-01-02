using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Effector : MonoBehaviour {
	
	[Range(0,100)]
	public float slowPercentage;
	public float slowDuration;
	private SlowEffect se;

	public float damage;
	private DamageEffect de;
	
	void OnTriggerEnter(Collider mob)
	{			
		if (mob.gameObject.GetComponent<EntityController>() != null) {			
			ApplyBaseEffect (mob);
		}
	}

	void OnTriggerExit(Collider mob)
	{
		if (mob.gameObject.GetComponent<EntityController>() != null) {
			if (this.gameObject.tag == "TowerBase")
				RemoveBaseEffect (mob);
		}
	}

	void ApplySlowEffect(Collider mob)
	{
		se = mob.gameObject.AddComponent <SlowEffect> () as SlowEffect;
		se.duration = slowDuration;
		se.ec = mob.gameObject.GetComponent<EntityController> ();
		se.slowPercentage = slowPercentage;
		se.Apply ();
	}

	void ApplyBaseEffect(Collider mob)
	{		
			//Debug.Log ("entity entering");
			if (slowPercentage > 0) {	
				if (mob.gameObject.GetComponent<SlowEffect> () != null) {
					if (mob.gameObject.GetComponent<SlowEffect> ().slowPercentage < slowPercentage) {
						Destroy (mob.gameObject.GetComponent<SlowEffect> ());
						ApplySlowEffect (mob);
					}
				} else {
					ApplySlowEffect(mob);
				}
			}

			if (damage > 0) {
				de = mob.gameObject.AddComponent<DamageEffect> () as DamageEffect;
				de.damage = damage;
				de.Apply ();
			}

	}

	void RemoveBaseEffect(Collider mob)
	{
		//Debug.Log ("entity leaving");
		if (slowPercentage > 0) {
			if ((se = mob.gameObject.GetComponent<SlowEffect> ()) != null)
				Component.Destroy (se);
		}
	}

}
