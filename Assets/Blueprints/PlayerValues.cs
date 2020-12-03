using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="PlayerValues")]
public class PlayerValues : DamageableConfig
{

    [Min(0)]
    public float currentHealth = 100;

    [Min(0)]
    public float goldCount = 0;
}
