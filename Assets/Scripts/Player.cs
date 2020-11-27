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
    GunConfig defaultGun;

    [SerializeField]
    int gunCapacity = 2;

    [SerializeField]
    GameEvent playerDeath;

    PlayerMovement playerMovement;

    float timeSinceLastShot = 0;

    int gunIndex = 0;

    List<GunPickup> nearbyGuns = new List<GunPickup>();

    PlayerHealth playerHealth;
    bool dying;
    void Start() {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = config as PlayerHealth;
        gunConfigs.Add(defaultGun);
        gun.config = gunConfigs[gunIndex];
    }

    void Update()
    {
        if(dying) {
            return;
        }
        if(Input.GetKeyDown(KeyCode.G)) {
            godModeEnabled = !godModeEnabled;
        }
        if(Input.GetKeyDown(KeyCode.P)) {
            TakeDamage(Random.Range(1,15));
        }
        PickupGun();
        EquipGun();
        
        if(Input.GetMouseButton(0) && timeSinceLastShot > gun.config.fireRate) {
            Shoot();
        }
        timeSinceLastShot += Time.deltaTime;
    }


    void EquipGun() {
        for(int i = 1; i <= gunConfigs.Count; i++) {
            if(Input.GetKeyDown("" + i)) {
                gunIndex = i - 1;
                gun.config = gunConfigs[gunIndex];
                return;
            }
        }
    }

    void PickupGun() {
        if(Input.GetKeyDown(KeyCode.E)) {
            if(nearbyGuns.Count > 0 && gunConfigs.Count < gunCapacity) {
                GunPickup pickup = nearbyGuns[nearbyGuns.Count - 1 ];
                gunConfigs.Add(pickup.config);
                gunIndex = gunConfigs.Count - 1;
                gun.config = gunConfigs[gunIndex];
                RemoveNearbyGun(pickup);
                pickup.Destroy();
            }
        }
    }

    public void AddNearbyGun(GunPickup pickup) {
        nearbyGuns.Add(pickup);

    }

    public void RemoveNearbyGun(GunPickup pickup) {
        nearbyGuns.Remove(pickup);
    }

    void Reset() {
        gunConfigs = new List<GunConfig>();
        gunConfigs.Add(defaultGun);
        gunIndex = 0;
        gun.config = gunConfigs[gunIndex];
        FullHeal();
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
        if(godModeEnabled || dying) {
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
        Reset();
        dying = false;
    }

}
