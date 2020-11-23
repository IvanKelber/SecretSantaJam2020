using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="DamageableConfig")]
public class DamageableConfig : ScriptableObject
{

    [Min(0)]
    public float maxHealth = 100;

    [Min(0)]
    public float currentHealth = 100;

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if(currentHealth < 0) {
            currentHealth = 0;
        }
    }

}
