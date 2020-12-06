﻿using System.Collections;
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
}
