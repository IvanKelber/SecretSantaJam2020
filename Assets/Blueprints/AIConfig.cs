using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="AIConfig")]
public class AIConfig : ScriptableObject
{
    [Range(1, 11)]
    public float movementSpeed = 3;

    [Range(1, 100)]
    public float detectionRadius = 15;

    [Range(1, 15)]
    public float maxAttackDistance = 10;

    public GunConfig gunConfig;



}
