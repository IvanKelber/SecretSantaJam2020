using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Player : Damageable
{

    [SerializeField]
    bool godModeEnabled = false;

    [SerializeField]
    List<GunConfig> gunConfigs = new List<GunConfig>();
    [SerializeField]
    Gun gun;

    [SerializeField]
    GameObject hand;

    [SerializeField]
    float handDistance;

    [SerializeField]
    GunConfig defaultGun;

    [SerializeField]
    int gunCapacity = 2;

    [SerializeField]
    GameEvent playerDeath;

    PlayerMovement playerMovement;

    float timeSinceLastShot = 0;

    int gunIndex = 0;

    bool waitingToRespawn = false;

    List<GunPickup> nearbyGuns = new List<GunPickup>();



    PlayerHealth playerHealth;
    bool dying;

    void Start() {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = config as PlayerHealth;
        gunConfigs.Add(defaultGun);
        EquipGun(0);
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
        GetEquipGun();
        
        if(playerMovement.GetShootKey() && timeSinceLastShot > gun.config.fireRate) {
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

    void GetEquipGun() {
        for(int i = 1; i <= gunConfigs.Count; i++) {
            if(Input.GetKeyDown("" + i)) {
                EquipGun(i - 1);
                return;
            }
        }
    }

    public void PickupGun() {
        if(nearbyGuns.Count > 0 && gunConfigs.Count < gunCapacity) {
            GunPickup pickup = nearbyGuns[nearbyGuns.Count - 1 ];
            gunConfigs.Add(pickup.config);
            EquipGun(gunConfigs.Count - 1);
            RemoveNearbyGun(pickup);
            pickup.Destroy();
        }
    }

    void EquipGun(int index) {
        gunIndex = index;
        gun.SetConfig(gunConfigs[gunIndex]);
    }

    public void AddNearbyGun(GunPickup pickup) {
        nearbyGuns.Add(pickup);

    }

    public void RemoveNearbyGun(GunPickup pickup) {
        nearbyGuns.Remove(pickup);
    }

    public void Reset() {
        if(waitingToRespawn) {
            gunConfigs = new List<GunConfig>();
            gunConfigs.Add(defaultGun);
            gunIndex = 0;
            gun.config = gunConfigs[gunIndex];
            FullHeal();
            waitingToRespawn = false;
        }
    }

    void FullHeal() {
        playerHealth.currentHealth = playerHealth.maxHealth;
        base.Heal(playerHealth.maxHealth);
    }

    void Shoot() {
        gun.Shoot(playerMovement.mousePosition);
        Vector3 knockbackDirection = (transform.position - playerMovement.mousePosition).normalized;
        Vector2 knockback = gun.config.knockback * new Vector2(knockbackDirection.x, knockbackDirection.y) * Time.deltaTime;
        playerMovement.Move(knockback);
        timeSinceLastShot = 0;
    }

    public override void TakeDamage(float damage) {
        if(godModeEnabled || playerHealth.currentHealth <= 0) {
            return;
        }
        base.TakeDamage(damage);
        audioManager.Play("PlayerGrunt", audioSource);
        playerHealth.currentHealth = currentHealth;
    }

    protected override void Die() {
        dying = true;
        audioManager.Play("PlayerDeath", audioSource);
        playerDeath.Raise();
        waitingToRespawn = true;
        dying = false;
    }

}
