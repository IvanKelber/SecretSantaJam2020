﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Configs/Damageable")]
public class DamageableConfig : ScriptableObject
{

    [Min(0)]
    public float maxHealth = 100;

}
