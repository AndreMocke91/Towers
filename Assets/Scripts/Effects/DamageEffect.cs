using UnityEngine;
using System.Collections;

public class DamageEffect : MonoBehaviour {

	public float damage = 1;

	private EntityController ec;

	public void Apply()
	{
		ec = GetComponent<EntityController> ();
		ec.health = ec.health - damage;
		Component.Destroy (this);
	}
}