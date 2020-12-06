using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using System.Reflection;

[CreateAssetMenu(menuName="PlayerValues")]
public class PlayerValues : ScriptableObject
{

    [Min(0)]
    public float maxHealth = 0;

    [Min(0)]
    public float currentHealth = 0;

    [Min(0)]
    public int maxArmor = 0;
    [Min(0)]
    public int currentArmor = 0;

    [Min(0)]
    public int goldCount = 0;

    public int projectileDamage = 0;
    public int projectileSpeed = 0;
    public float projectileSpread = 0;
    public int numberOfProjectilesPerShot = 0;

    public float projectileSpreadNoise = 10;

    public float projectileRange = 0;

    public float subsequentProjectileDelay = 0;

    public float shotsPerSecond = 0;

    public float playerMovementSpeed = 0;

    public Sprite gunSprite;

    public CinemachineImpulseDefinition fireImpulse;
    public BulletConfig projectile;

    public float onFireKnockback = 0;

    public float knockbackOnHit = 0;

    public float knockbackResistance = 0;

    public int projectileBounces = 0;

    public void Reset(PlayerValues other) {

        FieldInfo[] fieldInfo;
        Type playerValuesType = typeof(PlayerValues);
        // Get the type and fields of PlayerValues.
        fieldInfo = playerValuesType.GetFields(BindingFlags.Instance| BindingFlags.Public);
        for(int i = 0; i < fieldInfo.Length; i++)
        {
            fieldInfo[i].SetValue(this, fieldInfo[i].GetValue(other));
        }
    }

    public void Validate() {
        maxHealth = Mathf.Max(1, maxHealth);

        currentHealth = Mathf.Max(0, currentHealth);

        maxArmor = Mathf.Max(0, maxArmor);
        currentArmor = Mathf.Max(0, currentArmor);

        goldCount = Mathf.Max(0, goldCount);

        projectileDamage = Mathf.Max(1, projectileDamage);
        projectileSpeed = Mathf.Max(1, projectileSpeed);
        projectileSpread = Mathf.Max(0, projectileSpread);
        numberOfProjectilesPerShot = Mathf.Max(1, numberOfProjectilesPerShot);

        projectileSpreadNoise = Mathf.Max(0, projectileSpreadNoise);

        projectileRange = Mathf.Max(1, projectileRange);

        subsequentProjectileDelay = Mathf.Max(0, subsequentProjectileDelay);

        shotsPerSecond = Mathf.Max(.5f, shotsPerSecond);

        playerMovementSpeed = Mathf.Max(1, playerMovementSpeed);

        onFireKnockback = Mathf.Max(0, onFireKnockback);

        knockbackOnHit = Mathf.Max(0, knockbackOnHit);

        knockbackResistance = Mathf.Max(0, knockbackResistance);

        projectileBounces = Mathf.Max(0, projectileBounces); 
    }
}
