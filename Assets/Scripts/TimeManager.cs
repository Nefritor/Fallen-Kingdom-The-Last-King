using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

	public float slowdownSpeed = 0.05f, slowupSpeed = 0.01f;

	void Update () {
		if (!Input.GetButton ("Space")) {
			Time.timeScale += slowupSpeed;
			Time.timeScale = Mathf.Clamp (Time.timeScale, 0, 1);
		}
	}

	public void DoSlowMotion() {
		Time.timeScale = Mathf.Clamp (Time.timeScale, 0.1f, 1);
		Time.timeScale -= slowdownSpeed;
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}
}
