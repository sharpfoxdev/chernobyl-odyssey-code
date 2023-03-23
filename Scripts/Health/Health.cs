using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component, that is attached to both, enemy and player, so both can take damage
/// and heal. Although they are using different health bars, player on HUD, enemy floating one. 
/// </summary>
public class Health : MonoBehaviour {
    // object gets damaged every tick during continuous damage
    private WaitForSeconds damageTick = new WaitForSeconds(0.05f);
    private float currentHealth;
    private Coroutine damageCoroutine;

    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private bool destroyUponZero = false; //used for enemies

    /// <summary>
    /// Setup of variables and healthbar. We have to do this in Awake(), because player can change
    /// health value in Start() depending on whether he respawned or not. 
    /// </summary>
    public void Awake() {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    /// <summary>
    /// Sets current health to the given value
    /// </summary>
    /// <param name="health">New health value</param>
    public void SetHealth(float health) {
        currentHealth = health;
        CheckHealthBoundaries();
        UpdateHealthBar();
    }

    /// <summary>
    /// Removes HP from the current health and updates healthbar
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage) {
        currentHealth -= damage;
        CheckHealthBoundaries();
        UpdateHealthBar();
    }

    /// <summary>
    /// Starts coroutine of taking damage over time
    /// </summary>
    public void StartDamageOverTime() {
        damageCoroutine = StartCoroutine(TakeDamageOverTimeCoroutine());
    }

    /// <summary>
    /// Stops coroutine of taking damage over time
    /// </summary>
    public void StopDamageOverTime() {
        if(damageCoroutine != null) {
            //TakeDamageOverTimeCoroutine is running
            StopCoroutine(damageCoroutine);
        }
    }

    /// <summary>
    /// Coroutine of taking damage over time
    /// </summary>
    /// <returns></returns>
    private IEnumerator TakeDamageOverTimeCoroutine() {
        while(currentHealth > 0) {
            currentHealth -= maxHealth / 100; //every iteration we subtract onehunderthth of bar
            CheckHealthBoundaries();
            UpdateHealthBar();
            yield return damageTick; //every iteration stops here for 0.1 sec
        }
    }

    /// <summary>
    /// Adds HP to the current health and updates healthbar. Used after collecting medkit. 
    /// </summary>
    /// <param name="health">Amount of HP to heal. </param>
    public void Heal(float health) {
        currentHealth += health;
        CheckHealthBoundaries();
        UpdateHealthBar();
    }

    /// <summary>
    /// Updates healthbar with current health
    /// </summary>
    private void UpdateHealthBar() {
        healthBar.value = currentHealth;
    }

    /// <summary>
    /// Checks and fixes currentHealth value in a case it is 
    /// not between 0 and maxHealth
    /// </summary>
    private void CheckHealthBoundaries() {
        if(currentHealth <= 0) {
            if (destroyUponZero) {
                Destroy(gameObject);
            }
            currentHealth = 0;
        }
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }
    /// <summary>
    /// Used in Player script to check, whether it should die and respawn. 
    /// </summary>
    /// <returns></returns>
    public bool HealthIsZero() {
        if(currentHealth <= 0) {
            return true;
        }
        else {
            return false;
        }
    }
}
