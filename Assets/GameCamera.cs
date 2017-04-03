using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour {

	Vector3 cameraTarget;
	Quaternion cameraRotTarget;

	Transform target;

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void FixedUpdate () {
		cameraTarget = new Vector3 (target.position.x, transform.position.y, target.position.z - 8);
		transform.position = Vector3.Lerp (transform.position, cameraTarget, Time.fixedDeltaTime * 8);
	}
}
