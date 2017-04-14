using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycle : MonoBehaviour {

	public float SunCycleSpeed = 10;
    public RectTransform sunRect;

    void Update () {
		transform.RotateAround (Vector3.zero, new Vector3 (6,6,6), SunCycleSpeed * Time.deltaTime);
		transform.LookAt (Vector3.zero);
        float xPos = -transform.position.x + transform.position.z;
        sunRect.localPosition = new Vector3(xPos / 8, transform.position.y / 15 - 60, 0);
    }
}
