using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonCycle : MonoBehaviour {

	public float MoonCycleSpeed = 15;
	public Transform sunLight;
    public RectTransform moonRect;

    void Update () {
		transform.RotateAround (Vector3.zero, new Vector3 (2,5,4), MoonCycleSpeed * Time.deltaTime);
		transform.LookAt (Vector3.zero);

        float xPos = -transform.position.x + transform.position.z;
        moonRect.localPosition = new Vector3(xPos / 8, transform.position.y / 15 - 60, 0);

        if (sunLight.position.y >= 0) {
            if (GetComponent<Light>().shadowStrength >= 0.5 * Time.deltaTime)
			GetComponent<Light> ().shadowStrength -= 0.5f * Time.deltaTime;
		} else GetComponent<Light> ().shadowStrength += 0.5f * Time.deltaTime;
		GetComponent<Light> ().shadowStrength = Mathf.Clamp (GetComponent<Light> ().shadowStrength, 0, 0.5f);
	}
}
