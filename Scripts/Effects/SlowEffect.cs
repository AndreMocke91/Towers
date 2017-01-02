using UnityEngine;
using System.Collections;

public class SlowEffect : MonoBehaviour {

	public float slowPercentage = 50;
	public float duration = 0;

	public EntityController ec;
	public float initialSpeed;

	public SlowEffect(float Duration)
	{
		this.duration = Duration;
	}

	void OnDestroy()
	{
		if (ec != null)
			ec.speed = initialSpeed;
	}

	public void Apply()
	{
		if (ec != null) {
			Debug.Log ("Applying slow effect to " + ec.name);
			ec = GetComponent<EntityController> ();
			initialSpeed = ec.speed;
			ec.speed = (ec.speed - (ec.speed * slowPercentage / 100));
			if (duration != 0) {
				Invoke ("RemoveEffect", duration);
			}
		}
	}

	void RemoveEffect()
	{
		Destroy (this);
	}
}
