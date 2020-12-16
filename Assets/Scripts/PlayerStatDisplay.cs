using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatDisplay : MonoBehaviour
{
    public PlayerValues playerValues;

    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text armorText;
    public TMP_Text projectilesText;
    public TMP_Text projectileSpeedText;
    public TMP_Text accuracyText;
    public TMP_Text rangeText;
    public TMP_Text shotsPerSecondText;
    public TMP_Text movementSpeedText;
    public TMP_Text enemyKnockbackText;
    public TMP_Text recoilText;
    public TMP_Text knockbackResistanceText;
    public TMP_Text bouncesText;
    public TMP_Text dodgeCooldownText;

    CanvasGroup cg;

    public void Start() {
        cg = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        UpdateHealth();
        UpdateDamage();
        UpdateArmor();
        UpdateProjectiles();
        UpdateProjectileSpeed();
        UpdateAccuracy();
        UpdateRange();
        UpdateSPS();
        UpdateMovementSpeed();
        UpdateEnemyKnockback();
        UpdateRecoil();
        UpdateKnockbackResistance();
        UpdateBounces();
        UpdateDodgeCooldown();

        if(!StaticUserControls.paused && Input.GetKeyDown(KeyCode.Tab)) {
            ToggleVisibility();
        }
    }

    void ToggleVisibility() {
        cg.alpha = (cg.alpha + 1) % 2;
    }


    void UpdateHealth() {
        healthText.text = "Health: " + playerValues.currentHealth + "/" + playerValues.maxHealth;
    }
    void UpdateDamage() {
        damageText.text = "Damage: " + playerValues.projectileDamage;
    }
    void UpdateArmor() {
        armorText.text = "Armor: " + playerValues.currentArmor + "/" + playerValues.maxArmor;
    }
    void UpdateProjectiles() {
        projectilesText.text = "Projectiles: " + playerValues.numberOfProjectilesPerShot;
    }
    void UpdateProjectileSpeed() {
        projectileSpeedText.text = "Projectile Speed: " + playerValues.projectileSpeed;
    }
    void UpdateAccuracy() {
        accuracyText.text = "Accuracy: " + (int)Mathf.Clamp((100 - 100 * playerValues.projectileSpreadNoise/360),0,100);
    }
    void UpdateRange() {
        rangeText.text = "Range: " + playerValues.projectileRange;
    }
    void UpdateSPS() {
        shotsPerSecondText.text = "Shots Per Second: " + playerValues.shotsPerSecond; 
    }
    void UpdateMovementSpeed() {
        movementSpeedText.text = "Movement Speed: " + playerValues.playerMovementSpeed;
    }
    void UpdateEnemyKnockback() {
        enemyKnockbackText.text = "Enemy Knockback: " + (int) playerValues.knockbackOnHit;
    }    
    void UpdateRecoil() {
        recoilText.text = "Recoil: " + playerValues.onFireKnockback;
    }    
    void UpdateKnockbackResistance() {
        knockbackResistanceText.text = "Knockback Resistance: " + (int) playerValues.knockbackResistance;
    }
    void UpdateBounces() {
        bouncesText.text = "Bounces: " + playerValues.projectileBounces;
    }
    void UpdateDodgeCooldown() {
        dodgeCooldownText.text = "Dodge Cooldown: " + playerValues.dodgeCooldown;
    }
}
