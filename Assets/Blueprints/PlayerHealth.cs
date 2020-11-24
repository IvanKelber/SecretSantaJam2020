using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="PlayerHealth")]
public class PlayerHealth : DamageableConfig
{

    [Min(0)]
    public float currentHealth = 100;
}
