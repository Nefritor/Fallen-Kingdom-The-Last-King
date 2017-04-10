using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntity : MonoBehaviour
{

    public float level,
            experience,
            maxExperience,
            skillpoints,
            health,
            maxHealth,
            mana,
            maxMana,
            stamina,
            maxStamina,
            hunger,
            hungerDownSpeed,
            sleepiness,
            sleepDownSpeed,
            strenght,
            intellect,
            endurance,
            sneakiness;

    float healthTemp, manaTemp, staminaTemp, hungerTemp, sleepTemp;

    Image healthFillAmount, manaFillAmount, staminaFillAmount, hungryFillAmount, sleepinessFillAmount;
    //public Text healthW, healthB, manaW, manaB, staminaW, staminaB, hungryW, hungryB, sleepW, sleepB;
    Text healthText, manaText, staminaText, hungerText, thirstText, sleepText;

    void Start()
    {
        healthFillAmount = GameObject.Find("Health").transform.FindChild("Bar Mask").gameObject.transform.FindChild("Bar Fill").GetComponent<Image>();
        manaFillAmount = GameObject.Find("Mana").transform.FindChild("Bar Mask").gameObject.transform.FindChild("Bar Fill").GetComponent<Image>();
        staminaFillAmount = GameObject.Find("Stamina").transform.FindChild("Bar Mask").gameObject.transform.FindChild("Bar Fill").GetComponent<Image>();
        hungryFillAmount = GameObject.Find("Hunger").transform.FindChild("Bar Mask").gameObject.transform.FindChild("Bar Fill").GetComponent<Image>();
        hungryFillAmount = GameObject.Find("Thirst").transform.FindChild("Bar Mask").gameObject.transform.FindChild("Bar Fill").GetComponent<Image>();
        sleepinessFillAmount = GameObject.Find("Sleepiness").transform.FindChild("Bar Mask").gameObject.transform.FindChild("Bar Fill").GetComponent<Image>();

        healthText = GameObject.Find("Health").transform.FindChild("Text").GetComponent<Text>();
        manaText = GameObject.Find("Mana").transform.FindChild("Text").GetComponent<Text>();
        staminaText = GameObject.Find("Stamina").transform.FindChild("Text").GetComponent<Text>();
        hungerText = GameObject.Find("Hunger").transform.FindChild("Text").GetComponent<Text>();
        thirstText = GameObject.Find("Thirst").transform.FindChild("Text").GetComponent<Text>();
        sleepText = GameObject.Find("Sleepiness").transform.FindChild("Text").GetComponent<Text>();

        level = 1;
        strenght = 1;
        intellect = 1;
        endurance = 1;
        sneakiness = 1;

        hungerDownSpeed = 0.03f;
        hunger = 100;
        sleepDownSpeed = 0.01f;
        sleepiness = 100;

        maxHealth = 100 + level * 3 + endurance * 2;
        maxMana = 100 + level * 2 + intellect * 3;
        maxStamina = 100 + level * 2 + strenght * 2 + endurance * 2;

        health = maxHealth;
        mana = maxMana;
        stamina = maxStamina;
    }

    void Update()
    {
        maxHealth = 100 + level * 3 + endurance * 2;
        maxMana = 100 + level * 2 + intellect * 3;
        maxStamina = 100 + level * 2 + strenght * 2 + endurance * 2;

        healthTemp = health;
        manaTemp = mana;
        staminaTemp = stamina;
        hungerTemp = hunger;
        sleepTemp = sleepiness;

        sleepiness -= sleepDownSpeed * Time.deltaTime;
        hunger -= hungerDownSpeed * Time.deltaTime;
        stamina += maxStamina / 7 * Time.deltaTime;
        mana += mana / 300 * Time.deltaTime;

        healthFillAmount.fillAmount = health / maxHealth;
        manaFillAmount.fillAmount = mana / maxMana;
        staminaFillAmount.fillAmount = stamina / maxStamina;
        hungryFillAmount.fillAmount = hunger / 100;
        sleepinessFillAmount.fillAmount = sleepiness / 100;

        /*healthB.text = healthW.text;
        manaB.text = manaW.text;
        staminaB.text = staminaW.text;
        hungryB.text = hungryW.text;
        sleepB.text = sleepW.text;*/

        ClampPlayerInfo();

        string accInfo = accCalc(health, healthTemp);
        healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth) + accInfo;
        accInfo = accCalc(mana, manaTemp);
        manaText.text = Mathf.Round(mana) + "/" + Mathf.Round(maxMana) + accInfo;
        accInfo = accCalc(stamina, staminaTemp);
        staminaText.text = Mathf.Round(stamina) + "/" + Mathf.Round(maxStamina) + accInfo;
        accInfo = accCalc(hunger, hungerTemp);
        hungerText.text = Mathf.Round(hunger) + "/100" + accInfo;
        accInfo = accCalc(sleepiness, sleepTemp);
        sleepText.text = Mathf.Round(sleepiness) + "/100" + accInfo;
    }

    string accCalc(float val1, float val2)
    {
        if (val1 < val2)
        {
            return " (▼)";
        }
        else if (val1 > val2)
        {
            return " (▲)";
        }
        else
        {
            return " (=)";
        }
    }

    public virtual void Eat(int mealLevel)
    {
        hunger += mealLevel * 10;
        health += mealLevel * maxHealth / 20;
    }

    public virtual void Drink(int drinkLevel)
    {
        mana += drinkLevel * 50;
    }

    public virtual void LevelUp()
    {
        level += 1;
        experience = experience - maxExperience;
        maxExperience += level * 200;
        health = maxHealth;
        mana = maxMana;
        stamina = maxStamina;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log("Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log("Player is Dead");
    }

    public void ClampPlayerInfo()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        mana = Mathf.Clamp(mana, 0, maxMana);
        if (mana > maxMana)
        {
            mana = maxMana;
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        experience = Mathf.Clamp(experience, 0, maxExperience);
        if (experience > maxExperience)
        {
            experience = maxExperience;
        }
        sleepiness = Mathf.Clamp(sleepiness, 0, 100);
        if (sleepiness > 100)
        {
            sleepiness = 100;
        }
        hunger = Mathf.Clamp(hunger, 0, 100);
        if (hunger > 100)
        {
            hunger = 100;
        }
    }
}
