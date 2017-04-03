using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycle : MonoBehaviour {

	public float SunCycleSpeed = 10;

	void Update () {
		transform.RotateAround (Vector3.zero, new Vector3 (6,0,6), SunCycleSpeed * Time.deltaTime);
		transform.LookAt (Vector3.zero);
	}
}
