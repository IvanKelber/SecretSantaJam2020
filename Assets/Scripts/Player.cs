using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

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

    PlayerMovement playerMovement;

    float timeSinceLastShot = 0;


    bool waitingToRespawn = true;

    AudioSource audioSource;

    [SerializeField]
    PlayerValues playerValues;
    [SerializeField]
    PlayerValues defaultPlayerValues;

    bool dying;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        playerMovement = GetComponent<PlayerMovement>();
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
        UpdateHand();
        
        if(playerMovement.GetShootKey() && timeSinceLastShot > playerValues.fireRate) {
            Shoot();
        }
        timeSinceLastShot += Time.deltaTime;
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
        }
    }


    void Shoot() {
        playerGun.Shoot(playerMovement.mousePosition);
        Vector3 knockbackDirection = (transform.position - playerMovement.mousePosition).normalized;
        Vector2 knockback = playerValues.onFireKnockback * new Vector2(knockbackDirection.x, knockbackDirection.y) * Time.deltaTime;
        playerMovement.Move(knockback);
        timeSinceLastShot = 0;
    }

    public void TakeDamage(float damage) {
        if(godModeEnabled || playerValues.currentHealth <= 0) {
            return;
        }
        playerValues.currentHealth -= damage;
        playerValues.currentHealth = Mathf.Clamp(playerValues.currentHealth, 0, playerValues.maxHealth);
        audioManager.Play("PlayerGrunt", audioSource);
    }

    protected void Die() {
        dying = true;
        StartCoroutine(audioManager.PlayAndWait("PlayerDeath", audioSource));
        playerDeath.Raise();
        waitingToRespawn = true;
        dying = false;
    }

    public void OnRewardChosen(GameObject reward) {
        RewardConfig rewardConfig = reward.GetComponent<Reward>().config;
        Debug.Log("Player received config: " + rewardConfig.name);
    }

}
