using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntityController : MonoBehaviour {

	public float health = 100;
	public int hungerEffect = 1;
	public Slider healthBar;
	public bool isHostile = false;
	[Range(0, 20)]
	public float speed;

	public bool isDead = false;

	void Start()
	{
		if (tag == "Player") {
			healthBar.maxValue = health;
			healthBar.value = health;
			//InvokeRepeating ("ApplyHunger", 5, 5);
		}		
	}

	void Update()
	{
		if (tag == "Player")
			healthBar.value = health;

		if (health <= 0 && !isDead) {
			isDead = true;
		}

		if (isDead) {			
			speed = 0;
			if (tag != "Player")
				Invoke ("KillObject", 20f);
			if (GetComponent<Animator> () != null) {
				Destroy (GetComponent<Animator> ());
				Utils.RotateModel (this.gameObject, new Vector3 (0, -1000, 0), 150);
			}
		}
	}

	void ApplyHunger()
	{
		health = health - hungerEffect;
	}

	void KillObject()
	{
		Destroy(this.gameObject);
	}

}
