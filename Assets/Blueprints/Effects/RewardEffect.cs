using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Reward")]
public class RewardEffect : ScriptableObject
{

    public int projectileDamage;
    public int projectileSpeed;
    public float projectileSpread;
    public int numberOfProjectilesPerShot;

    public float projectileSpreadNoise;

    public float projectileLifetime;

    public float fireRate;

    public float playerMovementSpeed;

    public float playerMaxHealth;
    public float playerCurrentHealth;

    public virtual void Apply(PlayerValues values) {
        
    }
}
