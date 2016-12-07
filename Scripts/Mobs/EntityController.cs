using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntityController : MonoBehaviour {

	public float health = 100;
	public int hungerEffect = 1;
	public Slider healthBar;
	public GameObject deathMessage;

	private bool isDead = false;

	void Start()
	{
		if (tag == "Player") {
			healthBar.maxValue = health;
			healthBar.value = health;
			InvokeRepeating ("ApplyHunger", 5, 5);
		}
		
	}

	void Update()
	{
		if (health <= 0 && !isDead) {
			isDead = true;
		}

		if (isDead) {
			PlayerController pc = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
			pc.speed = 0;

		}
	}

	void ApplyHunger()
	{
		health = health - hungerEffect;
		Debug.Log ("Health at : " + health);
		healthBar.value = health;
	}

	public void ApplyDamage(float damage)
	{
		health = health - damage;
		Debug.Log ("Health at : " + health);
	}

}
