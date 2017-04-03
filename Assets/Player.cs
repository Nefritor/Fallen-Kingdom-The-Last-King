using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

	Vector3 moveVelocity, direction, rollDirection, currentVelocityMod;
	bool isRolling, isRollingDown, isSneaking, swordUp;
	public float speed, acceleration = 5, moveSpeed = 5, sneakSpeed = 2, rotationSpeed = 1000, rollAcc;
	PlayerController controller;
	Quaternion targetRotation, targetRotationRoll;
	public Text movementUi, swordUi;
	public TimeManager timeManager;
	PlayerEntity playerEntity;
	public RectTransform hUi, mUi, sUi;

	void Start () {
		playerEntity = GetComponent<PlayerEntity> ();
		controller = GetComponent<PlayerController> ();
		isRolling = false;
		isRollingDown = false;
		isSneaking = false;
		swordUp = false;
		swordUi.text = "Sword Down";
	}

	void Update () {
		// Player movement input
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

		currentVelocityMod = Vector3.MoveTowards (currentVelocityMod, moveInput.normalized, acceleration * Time.deltaTime);
		moveVelocity = speed * currentVelocityMod;

		// Setting target for Rotation
		if (moveInput != Vector3.zero) {
			targetRotation = Quaternion.LookRotation (moveInput);
		}

		// Showing Player information Listener
		ShowPlayerInfoListener ();

		// Showing Abilities Listener
		ShowingAbilitiesListener(moveInput);

		// Sneaking Listener
		SneakingListener();

		// Sword up/down Listener
		SwordUpDownListener();

		// Rolling Listener
		RollingListener(moveInput);

		// Moving Listener
		controller.Move (moveVelocity);

		// Rotating Listener
		controller.Rotate (targetRotation, rotationSpeed);
	}

	void RollingListener(Vector3 moveInput){
		if (moveInput != Vector3.zero && Input.GetButtonDown ("Space") && !isRolling && !isRollingDown && playerEntity.stamina >= 15) {
			rollDirection = moveInput.normalized;
			targetRotationRoll = targetRotation;
			isRolling = true;
			rollAcc = 1;
			movementUi.text = "Rolling";
			playerEntity.stamina -= 15;
		}
		if (isRolling && rollAcc <= 4) {
			moveInput = Vector3.zero;
			moveVelocity = rollAcc * speed * rollDirection;
			targetRotation = targetRotationRoll;
			rollAcc *= 1.3f;
		} else if (isRolling && rollAcc > 4) {
			isRolling = false;
			isRollingDown = true;
			rollAcc = 4;
		} else if (isRollingDown && rollAcc <= 4 && rollAcc >= 2) {
			moveInput = Vector3.zero;
			moveVelocity = rollAcc * speed * rollDirection;
			targetRotation = targetRotationRoll;
			rollAcc *= 0.95f;
		} else if (isRollingDown && rollAcc < 2) {
			isRollingDown = false;
		}
	}

	void SwordUpDownListener(){
		if (Input.GetAxisRaw ("Mouse ScrollWheel") > 0) {
			swordUp = true;
			swordUi.text = "Sword Up";
		} else if (Input.GetAxisRaw ("Mouse ScrollWheel") < 0){
			swordUp = false;
			swordUi.text = "Sword Down";
		}
	}

	void SneakingListener(){
		if (Input.GetButton ("Sneak") && !isRolling && !isRollingDown) {
			isSneaking = true;
			movementUi.text = "Sneaking";
			speed = sneakSpeed;
		} else if (!Input.GetButton ("Sneak") && !isRolling && !isRollingDown) {
			isSneaking = false;
			movementUi.text = "Walking";
			speed = moveSpeed;
		}
	}

	void ShowingAbilitiesListener(Vector3 moveInput){
		if (moveInput == Vector3.zero && Input.GetButton ("Space")) {
			timeManager.DoSlowMotion ();
		}
	}

	void ShowPlayerInfoListener(){
		float posX = hUi.localPosition.x;
		if (Input.GetButton ("PlayerInfo")) {
			if (posX <= 91) {
				posX *= 1.03f;
			} else if (posX > 91) {
				posX = 3 + posX * 0.976f;
			}
			posX = Mathf.Clamp (posX, 61, 101);
		} else if (!Input.GetButton ("PlayerInfo")) {
			if (posX >= 71) {
				posX *= 0.97f;
			} else if (posX < 71) {
				posX = posX * 1.025f - 3;
			}
			posX = Mathf.Clamp (posX, 61, 101);
		}
		hUi.localPosition = new Vector3 (posX, hUi.localPosition.y, hUi.localPosition.z);
		mUi.localPosition = new Vector3 (posX, hUi.localPosition.y, hUi.localPosition.z);
		sUi.localPosition = new Vector3 (posX, hUi.localPosition.y, hUi.localPosition.z);
	}
}
