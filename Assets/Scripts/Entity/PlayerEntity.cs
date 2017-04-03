using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntity : MonoBehaviour {

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


	public Image healthFillAmount, manaFillAmount, staminaFillAmount, hungryFillAmount, sleepinessFillAmount;

	void Start(){
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

	void Update () {
		maxHealth = 100 + level * 3 + endurance * 2;
		maxMana = 100 + level * 2 + intellect * 3;
		maxStamina = 100 + level * 2 + strenght * 2 + endurance * 2;

		health = Mathf.Clamp (health, 0, maxHealth);
		mana = Mathf.Clamp (mana, 0, maxMana);
		stamina = Mathf.Clamp (stamina, 0, maxStamina);
		experience = Mathf.Clamp (experience, 0, maxExperience);
		sleepiness = Mathf.Clamp (sleepiness, 0, 100);
		hunger = Mathf.Clamp (hunger, 0, 100);

		sleepiness -= sleepDownSpeed * Time.deltaTime;
		hunger -= hungerDownSpeed * Time.deltaTime;
		stamina += maxStamina / 7 * Time.deltaTime;
		mana += mana / 300 * Time.deltaTime;

		healthFillAmount.fillAmount = health / maxHealth;
		manaFillAmount.fillAmount = mana / maxMana;
		staminaFillAmount.fillAmount = stamina / maxStamina;
		hungryFillAmount.fillAmount = hunger / 100;
		sleepinessFillAmount.fillAmount = sleepiness / 100;
	}

	public virtual void Eat(int mealLevel){
		hunger += mealLevel * 10;
		health += mealLevel * maxHealth / 20;
	}

	public virtual void Drink(string potionType, int potionLevel){
		if (potionType == "health") {
			health += potionLevel * 50;
		} else if (potionType == "mana") {
			mana += potionLevel * 50;
		}
	}

	public virtual void LevelUp(){
		level += 1;
		experience = experience - maxExperience;
		maxExperience += level * 200;
		health = maxHealth;
		mana = maxMana;
		stamina = maxStamina;
	}

	public virtual void TakeDamage(float damage){
		health -= damage;

		Debug.Log ("Current health: " + health);

		if (health <= 0) {
			Die ();
		}
	}

	public virtual void Die(){
		Debug.Log ("Player is Dead");
	}
}
