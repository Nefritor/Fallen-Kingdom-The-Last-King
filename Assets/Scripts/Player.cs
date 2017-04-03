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
	public RectTransform hUi, mUi, sUi, hngUi, slpUi, playerInfoUI;
	public CanvasRenderer  mainInfoText, secInfoText;
	int infoOpenAlg = 0;

	void Start () {
		playerEntity = GetComponent<PlayerEntity> ();
		controller = GetComponent<PlayerController> ();
		isRolling = false;
		isRollingDown = false;
		isSneaking = false;
		swordUp = false;
		swordUi.text = "Sword Down";
		mainInfoText.SetAlpha (0);
		secInfoText.SetAlpha (0);
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
		float alpha = mainInfoText.GetAlpha ();
		float posY = playerInfoUI.localPosition.y;
		if (Input.GetButton ("PlayerInfo") && infoOpenAlg == 0) {
			posY += -(Mathf.Pow (posY - 30, 2) - 900) * 0.004f + 0.1f;
			posY = Mathf.Clamp (posY, 0, 60);
			if (posY >= 60) {
				infoOpenAlg = 1;
			}
		} else if (!Input.GetButton ("PlayerInfo") && infoOpenAlg == 0) {
			posY -= -(Mathf.Pow (posY -30, 2) - 900) * 0.004f + 0.1f;
			posY = Mathf.Clamp (posY, 0, 60);
		} else if (Input.GetButton ("PlayerInfo") && infoOpenAlg == 1) {
			if (alpha >= 1) {
				alpha = 1;
			} else {
				alpha += -(Mathf.Pow (alpha - 0.5f, 2) - 0.25f) * 0.008f + 0.1f;
			}
			posX += -(Mathf.Pow (posX - 81, 2) - 400) * 0.008f + 0.1f;
			posX = Mathf.Clamp (posX, 61, 101);
			mainInfoText.SetAlpha (alpha);
			secInfoText.SetAlpha (alpha);
		} else if (!Input.GetButton ("PlayerInfo") && infoOpenAlg == 1) {
			if (alpha <= 0) {
				alpha = 0;
			} else {
				alpha -= -(Mathf.Pow (alpha - 0.5f, 2) - 0.25f) * 0.008f + 0.1f;
			}
			posX -= -(Mathf.Pow (posX - 81, 2) - 400) * 0.008f + 0.1f;
			posX = Mathf.Clamp (posX, 61, 101);
			mainInfoText.SetAlpha (alpha);
			secInfoText.SetAlpha (alpha);
			if (posX <= 61) {
				infoOpenAlg = 0;
			}
		} 
		hUi.localPosition = new Vector3 (posX, hUi.localPosition.y, hUi.localPosition.z);
		mUi.localPosition = new Vector3 (posX, hUi.localPosition.y, hUi.localPosition.z);
		sUi.localPosition = new Vector3 (posX, hUi.localPosition.y, hUi.localPosition.z);
		hngUi.localPosition = new Vector3 (posX, hUi.localPosition.y, hUi.localPosition.z);
		slpUi.localPosition = new Vector3 (posX, hUi.localPosition.y, hUi.localPosition.z);
		playerInfoUI.localPosition = new Vector3 (playerInfoUI.localPosition.x, posY, playerInfoUI.localPosition.z);
	}
}
