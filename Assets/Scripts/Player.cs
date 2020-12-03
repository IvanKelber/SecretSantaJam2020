using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Player : Damageable
{

    [SerializeField]
    bool godModeEnabled = false;

    [SerializeField]
    GameObject hand;

    [SerializeField]
    float handDistance;


    [SerializeField]
    GameEvent playerDeath;

    WeaponInventory weaponInventory;

    PlayerMovement playerMovement;

    float timeSinceLastShot = 0;


    bool waitingToRespawn = true;


    PlayerValues playerValues;
    bool dying;

    void Start() {
        base.Start();
        weaponInventory = GetComponent<WeaponInventory>();
        playerMovement = GetComponent<PlayerMovement>();
        playerValues = config as PlayerValues;
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
        
        if(playerMovement.GetShootKey() && timeSinceLastShot > weaponInventory.Gun.config.fireRate) {
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


    public void PickupGun(GunConfig config) {
        weaponInventory.Pickup(config);
    }

    public void Reset() {
        if(waitingToRespawn) {
            weaponInventory.Reset();
            playerValues.goldCount = 0;
            FullHeal();
            waitingToRespawn = false;
        }
    }

    void FullHeal() {
        playerValues.currentHealth = playerValues.maxHealth;
        base.Heal(playerValues.maxHealth);
    }

    void Shoot() {
        weaponInventory.Gun.Shoot(playerMovement.mousePosition);
        Vector3 knockbackDirection = (transform.position - playerMovement.mousePosition).normalized;
        Vector2 knockback = weaponInventory.Gun.config.knockback * new Vector2(knockbackDirection.x, knockbackDirection.y) * Time.deltaTime;
        playerMovement.Move(knockback);
        timeSinceLastShot = 0;
    }

    public override void TakeDamage(float damage) {
        if(godModeEnabled || playerValues.currentHealth <= 0) {
            return;
        }
        base.TakeDamage(damage);
        audioManager.Play("PlayerGrunt", audioSource);
        playerValues.currentHealth = currentHealth;
    }

    protected override void Die() {
        dying = true;
        audioManager.Play("PlayerDeath", audioSource);
        playerDeath.Raise();
        waitingToRespawn = true;
        dying = false;
    }

    public void OnRewardChosen(GameObject reward) {
        RewardConfig rewardConfig = reward.GetComponent<Reward>().config;
        Debug.Log("Player received config: " + rewardConfig.name);
    }

}
