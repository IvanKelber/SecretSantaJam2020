using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[CreateAssetMenu(menuName="Effects/Reward")]
public class RewardEffect : PlayerValues
{
    public string effectText;
    public string adjective;
    public string descriptor;
    public float value;

    public string name;

    public void OnEnable() {
        name = adjective + " " + descriptor;
    }

    public virtual void Apply(PlayerValues values) {
        Apply(1, values);
    }

    public virtual void Apply(int scalar, PlayerValues values) {
        values.maxHealth += maxHealth * scalar;

        values.currentHealth += currentHealth * scalar;

        values.goldCount += goldCount * scalar;

        values.projectileDamage += projectileDamage * scalar;
        values.projectileSpeed += projectileSpeed * scalar;
        values.projectileSpread += projectileSpread * scalar;
        values.numberOfProjectilesPerShot += numberOfProjectilesPerShot * scalar;

        values.projectileSpreadNoise += projectileSpreadNoise * scalar;

        values.projectileRange += projectileRange * scalar;

        values.subsequentProjectileDelay += subsequentProjectileDelay * scalar;

        values.shotsPerSecond += shotsPerSecond * scalar;

        values.playerMovementSpeed += playerMovementSpeed * scalar;

        values.onFireKnockback += onFireKnockback * scalar;
        values.maxArmor += maxArmor * scalar;
        values.knockbackOnHit += knockbackOnHit * scalar;
        values.projectileBounces += projectileBounces * scalar;  
        values.knockbackResistance += knockbackResistance * scalar;
        values.dodgeCooldown += dodgeCooldown * scalar;
    }
}
