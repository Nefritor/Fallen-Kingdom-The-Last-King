using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Quaternion rotation;
	float rotationSpeed;
	Vector3 velocity;
	Rigidbody myRigitbody;

	void Start () {
		myRigitbody = GetComponent<Rigidbody> ();
	}

	public void Rotate(Quaternion _rotation, float _rotationSpeed){
		rotation = _rotation;
		rotationSpeed = _rotationSpeed;
	}

	public void Move(Vector3 _velocity){
		velocity = _velocity;
	}

	void FixedUpdate(){
		myRigitbody.MovePosition (myRigitbody.position + velocity * Time.fixedDeltaTime);
		myRigitbody.transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, rotation.eulerAngles.y, rotationSpeed * Time.fixedDeltaTime);
	}
}
