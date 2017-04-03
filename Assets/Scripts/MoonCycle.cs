using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonCycle : MonoBehaviour {

	public float MoonCycleSpeed = 3;
	public Transform sunLight;

	void Update () {
		transform.RotateAround (Vector3.zero, new Vector3 (2,5,4), MoonCycleSpeed * Time.deltaTime);
		transform.LookAt (Vector3.zero);
		if (sunLight.position.y >= 0) {
			GetComponent<Light> ().shadowStrength -= 0.5f * Time.deltaTime;
		} else GetComponent<Light> ().shadowStrength += 0.5f * Time.deltaTime;
		GetComponent<Light> ().shadowStrength = Mathf.Clamp (GetComponent<Light> ().shadowStrength, 0, 0.5f);
	}
}
