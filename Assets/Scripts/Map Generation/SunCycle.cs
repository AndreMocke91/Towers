using UnityEngine;
using System.Collections;

public class SunCycle : MonoBehaviour {

	public float dayTimeMinutes = 8;

	public float degreesPerSecond;
	// Use this for initialization
	void Start () {
		degreesPerSecond = 360 / (dayTimeMinutes * 60);
		degreesPerSecond /= 10;
		InvokeRepeating ("RotateSun", 0, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void RotateSun()
	{
		transform.Rotate (new Vector3 (degreesPerSecond, 0, 0));
	}
}
