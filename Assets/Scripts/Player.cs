using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    Vector3 moveVelocity, direction, rollDirection, currentVelocityMod, mouseInput;
    public bool isBlocked;
    bool isRolling, isRollingDown, isSneaking, swordUp, isInventory;
    public float speed, acceleration = 5, moveSpeed = 5, sneakSpeed = 2, rotationSpeed = 1000, rollSpeed, mouseX, mouseY;
    PlayerController controller;
    Quaternion targetRotation, targetRotationRoll, tempTargetRotation;
    public Text movementUi, swordUi;
    public TimeManager timeManager;
    PlayerEntity playerEntity;
    public RectTransform hUi, mUi, sUi, hngUi, slpUi, playerInfoUI, swordUI, inventoryUI;
    public CanvasRenderer mainInfoText, secInfoText, hW, hB, mW, mB, sW, sB, hngW, hngB, slpW, slpB, vignette, vignetteBottom;
    int infoOpenAlg = 0, itemId;
    GameObject[] invItem;

    void Start()
    {
        invItem = GameObject.FindGameObjectsWithTag("invSlot");
        mouseInput = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
        //Cursor.visible = false;
        playerEntity = GetComponent<PlayerEntity>();
        controller = GetComponent<PlayerController>();
        isRolling = false;
        isRollingDown = false;
        isSneaking = false;
        swordUp = false;
        swordUi.text = "Sword Down";
        rollSpeed = 15;
        inventoryUI.localPosition = new Vector3(inventoryUI.localPosition.x, -277, inventoryUI.localPosition.z);
        vignette.SetAlpha(0);
        mainInfoText.SetAlpha(0);
        secInfoText.SetAlpha(0);
        vignetteBottom.SetAlpha(0);
        hW.SetAlpha(0);
        hB.SetAlpha(0);
        mW.SetAlpha(0);
        mB.SetAlpha(0);
        sW.SetAlpha(0);
        sB.SetAlpha(0);
        hngW.SetAlpha(0);
        hngB.SetAlpha(0);
        slpW.SetAlpha(0);
        slpB.SetAlpha(0);
    }

    void Update()
    {
        // Player movement input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        currentVelocityMod = Vector3.MoveTowards(currentVelocityMod, moveInput.normalized, acceleration * Time.deltaTime);
        moveVelocity = speed * currentVelocityMod;

        // Setting target for Rotation
        if (moveInput != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(moveInput);
        }

        // Showing Player information Listener
        ShowPlayerInfoListener();

        // Showing Abilities Listener
        ShowingAbilitiesListener(moveInput);

        // Sneaking Listener
        SneakingListener();

        if (!isInventory)
        {
            // Sword up/down Listener
            SwordUpDownListener();
        }

        // Inventory open/close Listener
        InventoryListener();

        // Rolling Listener
        RollingListener(moveInput);

        // Moving Listener
        controller.Move(moveVelocity);

        // Rotating Listener
        controller.Rotate(targetRotation, rotationSpeed);
    }

    void RollingListener(Vector3 moveInput)
    {
        if (moveInput != Vector3.zero && Input.GetButtonDown("Space") && !isRolling && !isRollingDown && playerEntity.stamina >= 15)
        {
            rollDirection = moveInput.normalized;
            targetRotationRoll = targetRotation;
            isRolling = true;
            movementUi.text = "Rolling";
            playerEntity.stamina -= 15;
        }
        if (isRolling)
        {
            moveInput = Vector3.zero;
            moveVelocity = speed * rollDirection;
            targetRotation = targetRotationRoll;
            speed += -(Mathf.Pow(speed - (rollSpeed + moveSpeed) / 2, 2) - 60f) * Time.deltaTime;
            speed = Mathf.Clamp(speed, moveSpeed, rollSpeed);
            if (speed >= rollSpeed)
            {
                isRolling = false;
                isRollingDown = true;
            }
        }
        else if (isRollingDown)
        {
            moveInput = Vector3.zero;
            moveVelocity = speed * rollDirection;
            targetRotation = targetRotationRoll;
            speed -= -(Mathf.Pow(speed - (rollSpeed + moveSpeed) / 2, 2) - 40f) * Time.deltaTime;
            speed = Mathf.Clamp(speed, moveSpeed, rollSpeed);
            if (speed <= moveSpeed)
            {
                isRollingDown = false;
            }
        }
    }

    void SwordUpDownListener()
    {
        float swordY = swordUI.localPosition.y;
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            swordUp = true;
            swordUi.text = "Sword Up";
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            swordUp = false;
            swordUi.text = "Sword Down";
        }
        if (swordUp)
        {
            isBlocked = true;
            swordY += -(Mathf.Pow(swordY - 50, 2) - 1600) * 0.005f + 0.05f;
            swordY = Mathf.Clamp(swordY, 10, 90);
            swordUI.localPosition = new Vector3(swordUI.localPosition.x, swordY, swordUI.localPosition.z);
            if (swordY >= 90)
            {
                isBlocked = false;
            }
        }
        else if (!swordUp)
        {
            isBlocked = true;
            swordY -= -(Mathf.Pow(swordY - 50, 2) - 1600) * 0.005f + 0.05f;
            swordY = Mathf.Clamp(swordY, 10, 90);
            swordUI.localPosition = new Vector3(swordUI.localPosition.x, swordY, swordUI.localPosition.z);
            if (swordY <= 10)
            {
                isBlocked = false;
            }
        }
    }

    void InventoryListener()
    {
        float inventoryY = inventoryUI.localPosition.y;
        float alphaVign = vignetteBottom.GetAlpha();
        if (Input.GetButton("Inventory") && !isInventory && !isBlocked)
        {
            tempTargetRotation = targetRotation;
            isInventory = true;
        }
        else if (!Input.GetButton("Inventory") && isInventory)
        {
            isInventory = false;
        }
        if (isInventory)
        {
            inventoryY += -(Mathf.Pow(inventoryY + 226, 2) - 2601) * 0.003f + 0.05f;
            inventoryY = Mathf.Clamp(inventoryY, -277, -175);
            inventoryUI.localPosition = new Vector3(inventoryUI.localPosition.x, inventoryY, inventoryUI.localPosition.z);
            if (alphaVign >= 1)
            {
                alphaVign = 1;
            }
            else
            {
                alphaVign += -(Mathf.Pow(alphaVign - 0.5f, 2) - 0.25f) * 0.03f + 0.02f;
            }
            vignetteBottom.SetAlpha(alphaVign);
        }
        else if (!isInventory)
        {
            inventoryY -= -(Mathf.Pow(inventoryY + 226, 2) - 2601) * 0.003f + 0.05f;
            inventoryY = Mathf.Clamp(inventoryY, -277, -175);
            inventoryUI.localPosition = new Vector3(inventoryUI.localPosition.x, inventoryY, inventoryUI.localPosition.z);
            if (alphaVign <= 0)
            {
                alphaVign = 0;
            }
            else
            {
                alphaVign -= -(Mathf.Pow(alphaVign - 0.5f, 2) - 0.25f) * 0.03f + 0.02f;
            }
            vignetteBottom.SetAlpha(alphaVign);
        }
    }

    void SneakingListener()
    {
        if (Input.GetButton("Sneak") && !isRolling && !isRollingDown)
        {
            isSneaking = true;
            movementUi.text = "Sneaking";
            speed = sneakSpeed;
        }
        else if (!Input.GetButton("Sneak") && !isRolling && !isRollingDown)
        {
            isSneaking = false;
            movementUi.text = "Walking";
            speed = moveSpeed;
        }
    }

    void ShowingAbilitiesListener(Vector3 moveInput)
    {
        if (moveInput == Vector3.zero && Input.GetButton("Space"))
        {
            timeManager.DoSlowMotion();
        }
    }

    void ShowPlayerInfoListener()
    {
        float posX = hUi.localPosition.x;
        float alpha = mainInfoText.GetAlpha();
        float alpha1 = hW.GetAlpha();
        float alphaVign = vignette.GetAlpha();
        float posY = playerInfoUI.localPosition.y;
        if (Input.GetButton("PlayerInfo") && infoOpenAlg == 0)
        {
            posY += -(Mathf.Pow(posY - 30, 2) - 900) * 0.004f + 0.1f;
            posY = Mathf.Clamp(posY, 0, 60);
            if (alphaVign >= 1)
            {
                alphaVign = 1;
            }
            else
            {
                alphaVign += -(Mathf.Pow(alphaVign - 0.5f, 2) - 0.25f) * 0.008f + 0.02f;
            }
            vignette.SetAlpha(alphaVign);
            if (posY >= 60)
            {
                infoOpenAlg = 1;
            }
        }
        else if (!Input.GetButton("PlayerInfo") && infoOpenAlg == 0)
        {
            posY -= -(Mathf.Pow(posY - 30, 2) - 900) * 0.004f + 0.1f;
            posY = Mathf.Clamp(posY, 0, 60);
            if (alphaVign <= 0)
            {
                alphaVign = 0;
            }
            else
            {
                alphaVign -= -(Mathf.Pow(alphaVign - 0.5f, 2) - 0.25f) * 0.008f + 0.02f;
            }
            vignette.SetAlpha(alphaVign);
        }
        else if (Input.GetButton("PlayerInfo") && infoOpenAlg == 1)
        {
            if (alpha >= 1)
            {
                alpha = 1;
            }
            else
            {
                alpha += -(Mathf.Pow(alpha - 0.5f, 2) - 0.25f) * 0.008f + 0.05f;
            }
            posX += -(Mathf.Pow(posX - 81, 2) - 400) * 0.008f + 0.1f;
            posX = Mathf.Clamp(posX, 61, 101);
            mainInfoText.SetAlpha(alpha);
            secInfoText.SetAlpha(alpha);
            if (posX >= 101)
            {
                infoOpenAlg = 2;
            }
        }
        else if (!Input.GetButton("PlayerInfo") && infoOpenAlg == 1)
        {
            if (alpha <= 0)
            {
                alpha = 0;
            }
            else
            {
                alpha -= -(Mathf.Pow(alpha - 0.5f, 2) - 0.25f) * 0.008f + 0.05f;
            }
            posX -= -(Mathf.Pow(posX - 81, 2) - 400) * 0.008f + 0.1f;
            posX = Mathf.Clamp(posX, 61, 101);
            mainInfoText.SetAlpha(alpha);
            secInfoText.SetAlpha(alpha);
            if (posX <= 61)
            {
                infoOpenAlg = 0;
            }
        }
        else if (Input.GetButton("PlayerInfo") && infoOpenAlg == 2)
        {
            if (alpha1 >= 1)
            {
                alpha1 = 1;
            }
            else
            {
                alpha1 += -(Mathf.Pow(alpha1 - 0.5f, 2) - 0.25f) * 0.008f + 0.05f;
            }
            hW.SetAlpha(alpha1);
            hB.SetAlpha(alpha1);
            mW.SetAlpha(alpha1);
            mB.SetAlpha(alpha1);
            sW.SetAlpha(alpha1);
            sB.SetAlpha(alpha1);
            hngW.SetAlpha(alpha1);
            hngB.SetAlpha(alpha1);
            slpW.SetAlpha(alpha1);
            slpB.SetAlpha(alpha1);
        }
        else if (!Input.GetButton("PlayerInfo") && infoOpenAlg == 2)
        {
            if (alpha1 <= 0)
            {
                alpha1 = 0;
            }
            else
            {
                alpha1 -= -(Mathf.Pow(alpha1 - 0.5f, 2) - 0.25f) * 0.008f + 0.05f;
            }
            hW.SetAlpha(alpha1);
            hB.SetAlpha(alpha1);
            mW.SetAlpha(alpha1);
            mB.SetAlpha(alpha1);
            sW.SetAlpha(alpha1);
            sB.SetAlpha(alpha1);
            hngW.SetAlpha(alpha1);
            hngB.SetAlpha(alpha1);
            slpW.SetAlpha(alpha1);
            slpB.SetAlpha(alpha1);
            if (alpha1 <= 0)
            {
                infoOpenAlg = 1;
            }
        }
        hUi.localPosition = new Vector3(posX, hUi.localPosition.y, hUi.localPosition.z);
        mUi.localPosition = new Vector3(posX, hUi.localPosition.y, hUi.localPosition.z);
        sUi.localPosition = new Vector3(posX, hUi.localPosition.y, hUi.localPosition.z);
        hngUi.localPosition = new Vector3(posX, hUi.localPosition.y, hUi.localPosition.z);
        slpUi.localPosition = new Vector3(posX, hUi.localPosition.y, hUi.localPosition.z);
        playerInfoUI.localPosition = new Vector3(playerInfoUI.localPosition.x, posY, playerInfoUI.localPosition.z);
    }

    public Vector3 playerPosition()
    {
        Vector3 myPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        return myPosition;
    }
}
