using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

    RectTransform playerInfo, healthRect, manaRect, staminaRect, hungerRect, thirstRect, sleepRect;
    CanvasRenderer healthText, manaText, staminaText, hungerText, thirstText, sleepText,
        healthTextInfo, manaTextInfo, staminaTextInfo, hungerTextInfo, thirstTextInfo, sleepTextInfo,
        healthIcon, manaIcon, staminaIcon, hungerIcon, thirstIcon, sleepIcon;
    float infoOpenAlg;

    void Start () {
        playerInfo = GameObject.Find("PlayerInfo").GetComponent<RectTransform>();
        healthRect = GameObject.Find("Health").transform.FindChild("Bar Mask").GetComponent<RectTransform>();
        manaRect = GameObject.Find("Mana").transform.FindChild("Bar Mask").GetComponent<RectTransform>();
        staminaRect = GameObject.Find("Stamina").transform.FindChild("Bar Mask").GetComponent<RectTransform>();
        hungerRect = GameObject.Find("Hunger").transform.FindChild("Bar Mask").GetComponent<RectTransform>();
        thirstRect = GameObject.Find("Thirst").transform.FindChild("Bar Mask").GetComponent<RectTransform>();
        sleepRect = GameObject.Find("Sleepiness").transform.FindChild("Bar Mask").GetComponent<RectTransform>();

        healthText = GameObject.Find("Health").transform.FindChild("Text").GetComponent<CanvasRenderer>();
        manaText = GameObject.Find("Mana").transform.FindChild("Text").GetComponent<CanvasRenderer>();
        staminaText = GameObject.Find("Stamina").transform.FindChild("Text").GetComponent<CanvasRenderer>();
        hungerText = GameObject.Find("Hunger").transform.FindChild("Text").GetComponent<CanvasRenderer>();
        thirstText = GameObject.Find("Thirst").transform.FindChild("Text").GetComponent<CanvasRenderer>();
        sleepText = GameObject.Find("Sleepiness").transform.FindChild("Text").GetComponent<CanvasRenderer>();

        healthTextInfo = GameObject.Find("Health").transform.FindChild("Text Info").GetComponent<CanvasRenderer>();
        manaTextInfo = GameObject.Find("Mana").transform.FindChild("Text Info").GetComponent<CanvasRenderer>();
        staminaTextInfo = GameObject.Find("Stamina").transform.FindChild("Text Info").GetComponent<CanvasRenderer>();
        hungerTextInfo = GameObject.Find("Hunger").transform.FindChild("Text Info").GetComponent<CanvasRenderer>();
        thirstTextInfo = GameObject.Find("Thirst").transform.FindChild("Text Info").GetComponent<CanvasRenderer>();
        sleepTextInfo = GameObject.Find("Sleepiness").transform.FindChild("Text Info").GetComponent<CanvasRenderer>();

        healthIcon = GameObject.Find("Health").transform.FindChild("Icon").GetComponent<CanvasRenderer>();
        manaIcon = GameObject.Find("Mana").transform.FindChild("Icon").GetComponent<CanvasRenderer>();
        staminaIcon = GameObject.Find("Stamina").transform.FindChild("Icon").GetComponent<CanvasRenderer>();
        hungerIcon = GameObject.Find("Hunger").transform.FindChild("Icon").GetComponent<CanvasRenderer>();
        thirstIcon = GameObject.Find("Thirst").transform.FindChild("Icon").GetComponent<CanvasRenderer>();
        sleepIcon = GameObject.Find("Sleepiness").transform.FindChild("Icon").GetComponent<CanvasRenderer>();

        healthText.SetAlpha(0);
        manaText.SetAlpha(0);
        staminaText.SetAlpha(0);
        hungerText.SetAlpha(0);
        thirstText.SetAlpha(0);
        sleepText.SetAlpha(0);

        healthTextInfo.SetAlpha(0);
        manaTextInfo.SetAlpha(0);
        staminaTextInfo.SetAlpha(0);
        hungerTextInfo.SetAlpha(0);
        thirstTextInfo.SetAlpha(0);
        sleepTextInfo.SetAlpha(0);

        infoOpenAlg = 0;
    }

    private void Update()
    {
        float posX = healthRect.localPosition.x;
        float alphaText = healthText.GetAlpha();
        float alphaIcon = healthIcon.GetAlpha();
        float posY = playerInfo.localPosition.y;
        if (Input.GetButton("PlayerInfo") && infoOpenAlg == 0)
        {
            posY += -(Mathf.Pow(posY + 59.5f, 2) - 2760) * 0.0025f;
            posY = Mathf.Clamp(posY, -112, -7f);
            if (posY >= -8f)
            {
                infoOpenAlg = 1;
            }
        }
        else if (!Input.GetButton("PlayerInfo") && infoOpenAlg == 0)
        {
            posY -= -(Mathf.Pow(posY + 59.5f, 2) - 2760) * 0.0025f;
            posY = Mathf.Clamp(posY, -112, -7f);
        }

        else if (Input.GetButton("PlayerInfo") && infoOpenAlg == 1)
        {
            posX += -(Mathf.Pow(posX - 26f, 2) - 160) * 0.01f;
            posX = Mathf.Clamp(posX, 14f, 38f);
            alphaIcon -= -(Mathf.Pow(alphaIcon - 0.5f, 2) - 0.3f) * 0.5f;
            alphaIcon = Mathf.Clamp(alphaIcon, 0, 1);
            if (posX >= 38f)
            {
                infoOpenAlg = 2;
            }
        }
        else if (!Input.GetButton("PlayerInfo") && infoOpenAlg == 1)
        {
            posX -= -(Mathf.Pow(posX - 26f, 2) - 160) * 0.01f;
            posX = Mathf.Clamp(posX, 14f, 38f);
            alphaIcon += -(Mathf.Pow(alphaIcon - 0.5f, 2) - 0.3f) * 0.5f;
            alphaIcon = Mathf.Clamp(alphaIcon, 0, 1);
            if (posX <= 14f)
            {
                infoOpenAlg = 0;
            }
        }

        else if (Input.GetButton("PlayerInfo") && infoOpenAlg == 2)
        {
            alphaText += -(Mathf.Pow(alphaText - 0.5f, 2) - 0.3f) * 0.5f;
            alphaText = Mathf.Clamp(alphaText, 0, 1);
        }
        else if (!Input.GetButton("PlayerInfo") && infoOpenAlg == 2)
        {
            alphaText -= -(Mathf.Pow(alphaText - 0.5f, 2) - 0.3f) * 0.3f;
            alphaText = Mathf.Clamp(alphaText, 0, 1);
            if (alphaText <= 0)
            {
                infoOpenAlg = 1;
            }
        }

        playerInfo.localPosition = new Vector2(playerInfo.localPosition.x, posY);
        healthRect.localPosition = new Vector2(posX, healthRect.localPosition.y);
        manaRect.localPosition = new Vector2(posX, manaRect.localPosition.y);
        staminaRect.localPosition = new Vector2(posX, staminaRect.localPosition.y);
        hungerRect.localPosition = new Vector2(posX, hungerRect.localPosition.y);
        thirstRect.localPosition = new Vector2(posX, thirstRect.localPosition.y);
        sleepRect.localPosition = new Vector2(posX, sleepRect.localPosition.y);

        healthIcon.SetAlpha(alphaIcon);
        manaIcon.SetAlpha(alphaIcon);
        staminaIcon.SetAlpha(alphaIcon);
        hungerIcon.SetAlpha(alphaIcon);
        thirstIcon.SetAlpha(alphaIcon);
        sleepIcon.SetAlpha(alphaIcon);

        healthText.SetAlpha(alphaText);
        manaText.SetAlpha(alphaText);
        staminaText.SetAlpha(alphaText);
        hungerText.SetAlpha(alphaText);
        thirstText.SetAlpha(alphaText);
        sleepText.SetAlpha(alphaText);

        healthTextInfo.SetAlpha(alphaText);
        manaTextInfo.SetAlpha(alphaText);
        staminaTextInfo.SetAlpha(alphaText);
        hungerTextInfo.SetAlpha(alphaText);
        thirstTextInfo.SetAlpha(alphaText);
        sleepTextInfo.SetAlpha(alphaText);
    }
}
