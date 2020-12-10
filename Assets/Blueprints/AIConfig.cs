using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Configs/AI")]
public class AIConfig : ScriptableObject
{
    [Range(1, 10000)]
    public float baseMaxHealth = 10;

    [Range(1, 10000)]
    public float maxHealth = 10;

    [Range(1, 100)]
    public float movementSpeed = 3;

    [Range(1, 100)]
    public float detectionRadius = 15;

    [Range(1, 100)]
    public float maxAttackDistance = 10;

    [Range(.5f, 100)]
    public float minAttackDistance = 5;

    public GunConfig gunConfig;

    public GameObject spawnIndicator;



}
