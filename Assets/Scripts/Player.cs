using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable
{

    [SerializeField]
    bool godModeEnabled = false;

    [SerializeField]
    List<GunConfig> gunConfigs = new List<GunConfig>();
    [SerializeField]
    Gun gun;

    [SerializeField]
    int gunCapacity = 2;

    PlayerMovement playerMovement;

    float timeSinceLastShot = 0;

    int gunIndex = 0;

    List<GunPickup> nearbyGuns = new List<GunPickup>();

    PlayerHealth playerHealth;

    void Start() {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = config as PlayerHealth;
    }

    void Update()
    {
       
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

    void Shoot() {
       
        gun.Shoot(playerMovement.mousePosition);
        Vector3 knockbackDirection = (transform.position - playerMovement.mousePosition).normalized;
        Vector2 knockback = gun.config.knockback * new Vector2(knockbackDirection.x, knockbackDirection.y);
        playerMovement.Move(knockback);
        timeSinceLastShot = 0;
    }

    public override void TakeDamage(float damage) {
        currentHealth -= damage;
        playerHealth.currentHealth = currentHealth;
        if(currentHealth <= 0) {
            currentHealth = 0;
            Die();
        }
    }
}
