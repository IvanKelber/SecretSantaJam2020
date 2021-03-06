﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

public class Player : MonoBehaviour, IDamageable
{

    [SerializeField]
    bool godModeEnabled = false;

    [SerializeField]
    GameObject hand;

    [SerializeField]
    float handDistance;

    [SerializeField]
    GameEvent playerDeath;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    PlayerGun playerGun;

    [SerializeField]
    Renderer renderer;

    [SerializeField,
    Range(0,60)]
    int flashFrames = 10;

    [SerializeField,
    Range(0,60)]
    int freezeFrames = 10;

    PlayerMovement playerMovement;

    float timeSinceLastShot = 0;
    float timeSinceLastDodge = 0;

    bool waitingToRespawn = true;
    bool flashing = false;
    bool dodging = false;
    Color initialColor;

    [SerializeField, Range(.01f,10)]
    float invincibilityDuration = .5f;

    AudioSource audioSource;

    [SerializeField]
    PlayerValues playerValues;
    [SerializeField]
    PlayerValues defaultPlayerValues;

    [SerializeField]
    DodgeTimer dodgeTimer;

    [SerializeField]
    GameObject body;

    bool dying;

    void Start() {
        Cursor.visible = false;
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.SetPlayerValues(playerValues);
        timeSinceLastDodge = playerValues.dodgeCooldown;
        initialColor = renderer.material.color;

        Reset();
    }

    void Update()
    {
        if(dying) {
            return;
        }
        if(Input.GetKeyDown(KeyCode.G)) {
            godModeEnabled = !godModeEnabled;
        }
        playerValues.Validate();
        UpdateHand();
        
        if(playerMovement.GetDodgeKeyDown() && timeSinceLastDodge > playerValues.dodgeCooldown && !dodging) {
            StartCoroutine(Dodge());
        }
        if(!dodging && playerMovement.GetShootKey() && timeSinceLastShot > 1/playerValues.shotsPerSecond) {
            Shoot();
        }


        timeSinceLastShot += Time.deltaTime;
        timeSinceLastDodge += Time.deltaTime;
        dodgeTimer.UpdateTimer(timeSinceLastDodge);
    }

    IEnumerator Dodge() {
        dodging = true;
        timeSinceLastDodge = 0;
        audioManager.Play("Dodge", audioSource);
        float direction = playerMovement.flipped ? -180 : 180;
        Tween rotation = body.transform.DORotate(new Vector3(0, 0, direction), invincibilityDuration, RotateMode.LocalAxisAdd)
                                   .SetEase(Ease.OutBack);
        playerValues.playerMovementSpeed *= 1.5f;
        renderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, .5f);
        yield return rotation.WaitForCompletion();
        renderer.material.color = initialColor;
        playerValues.playerMovementSpeed /= 1.5f;
        dodging = false;
    }

    void UpdateHand() {
        if(StaticUserControls.paused) {
            return;
        }
        Vector3 direction = (playerMovement.mousePosition - transform.position).normalized;
        float distance = Mathf.Min(handDistance, Vector3.Distance(transform.position, playerMovement.mousePosition)/2);
        Vector3 handPosition = transform.position + direction * distance;
        hand.transform.position = handPosition;
        hand.transform.rotation = Quaternion.Euler(0,0, Vector3.SignedAngle(playerMovement.flipped ? Vector3.left : Vector3.right, direction, Vector3.forward));
    }

    public void Reset() {
        if(waitingToRespawn) {
            playerValues.Reset(defaultPlayerValues);
            waitingToRespawn = false;
            playerMovement.playerDead = false;
        }
    }


    void Shoot() {
        playerGun.Shoot(playerMovement.mousePosition + Vector3.up * .65f, transform.position);
        Vector3 knockbackDirection = (transform.position - playerMovement.mousePosition).normalized;
        float knockbackMagnitude = Mathf.Clamp(playerValues.onFireKnockback - playerValues.knockbackResistance, 0, playerValues.onFireKnockback);
        Vector2 knockback =  knockbackMagnitude * new Vector2(knockbackDirection.x, knockbackDirection.y);
        playerMovement.Force(knockback);
        timeSinceLastShot = 0;
    }

    public bool TakeDamage(float damage) {
        StartCoroutine(Flash());
        audioManager.Play("PlayerGrunt", audioSource);
        if(playerValues.currentArmor > 0) {
            float actualDamage = damage - playerValues.currentArmor;
            if(actualDamage < 0) {
                playerValues.currentArmor -= (int) damage;
                return true;
            } else {
                playerValues.currentArmor = 0;
                damage = actualDamage;
            }
        }
        playerValues.currentHealth -= damage;
        playerValues.currentHealth = Mathf.Clamp(playerValues.currentHealth, 0, playerValues.maxHealth);
        if(playerValues.currentHealth == 0) {
            Die();
        }
        return true;
    }

    public void ResetArmor() {
        playerValues.currentArmor = playerValues.maxArmor;
    }

    protected void Die() {
        Debug.Log("Dying");
        dying = true;
        playerMovement.playerDead = true;
        StartCoroutine(audioManager.PlayAndWait("PlayerDeath", audioSource));
        playerDeath.Raise();
        waitingToRespawn = true;
        Reset();
        dying = false;
    }

    public bool TakeDamage(float damage, Vector3 knockback) {
        if(godModeEnabled || playerValues.currentHealth <= 0 || flashing || dodging) {
            Debug.Log("No Damage Taken");
            return false;
        }
        float magnitude = knockback.magnitude;
        magnitude -= playerValues.knockbackResistance;
        magnitude = Mathf.Clamp(magnitude, 0, knockback.magnitude);
        playerMovement.Force(knockback.normalized * magnitude);
        return TakeDamage(damage);
    }

    IEnumerator Flash() {
        flashing = true;
        Time.timeScale = 0;
        renderer.material.color = Color.white;
        for(int i = 0; i < freezeFrames; i++) {
            yield return null;
        }
        renderer.material.color = initialColor;
        Time.timeScale = 1;
        Tween flash = renderer.material.DOColor(Color.white, invincibilityDuration).SetEase(Ease.Flash, 16, 0);
        yield return flash.WaitForCompletion();
        flashing = false;
    }


    public void OnRewardChosen(GameObject reward) {
        RewardConfig rewardConfig = reward.GetComponent<Reward>().config;
        if(playerValues.currentHealth <= 0) {
            Die();
            return;
        }
        ResetArmor();
    }

}
