using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[CreateAssetMenu(menuName="Effects/Reward")]
public class RewardEffect : PlayerValues
{
    public virtual void Apply(PlayerValues values) {
        // FieldInfo[] fieldInfo;
        // Type valuesType = typeof(PlayerValues);
        // // Get the type and fields of PlayerValues.
        // fieldInfo = valuesType.GetFields(BindingFlags.Instance| BindingFlags.Public);
        // for(int i = 0; i < fieldInfo.Length; i++)
        // {
        //     if(fieldInfo[i].FieldType == typeof(int) || fieldInfo[i].FieldType == typeof(float)) {
        //         dynamic current = Convert.ChangeType(fieldInfo[i].GetValue(values), fieldInfo[i].FieldType); 
        //         dynamic effect = Convert.ChangeType(fieldInfo[i].GetValue(this), fieldInfo[i].FieldType); 
        //         fieldInfo[i].SetValue(values, (Object) (current + effect));
        //     }
        // }

        values.maxHealth += maxHealth;

        values.currentHealth += currentHealth;

        values.goldCount += goldCount;

        values.projectileDamage += projectileDamage;
        values.projectileSpeed += projectileSpeed;
        values.projectileSpread += projectileSpread;
        values.numberOfProjectilesPerShot += numberOfProjectilesPerShot;

        values.projectileSpreadNoise += projectileSpreadNoise;

        values.projectileRange += projectileRange;

        values.subsequentProjectileDelay += subsequentProjectileDelay;

        values.shotsPerSecond += shotsPerSecond;

        values.playerMovementSpeed += playerMovementSpeed;

        values.onFireKnockback += onFireKnockback;
        values.maxArmor += maxArmor;
        values.knockbackOnHit += knockbackOnHit;

    }
}
