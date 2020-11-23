using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
[CreateAssetMenu(menuName="GunConfig")]
public class GunConfig : ScriptableObject
{
    [Range(0.05f, 1.5f)]
    public float fireRate = 1;

    public CinemachineImpulseDefinition fireImpulse;
    public string fireSound;

    [Range(0,100)]
    public float knockback = 0;

    [Range(0,90)]
    public float spreadNoise = 0;

    [Range(0, 90)]
    public float naturalSpread = 0;

    [Range(1,15)]
    public int numberOfBullets = 1;

    public BulletConfig bullet;
}
