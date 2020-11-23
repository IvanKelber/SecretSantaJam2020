using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
[CreateAssetMenu(menuName="GunConfig")]
public class GunConfig : ScriptableObject
{
    [Range(0.05f, 1.5f)]
    public float fireRate;

    public CinemachineImpulseDefinition fireImpulse;
    public string fireSound;

    [Range(0,100)]
    public float knockback;

    [Range(0,90)]
    public float spreadNoise;

    [Range(0, 90)]
    public float naturalSpread;

    [Range(1,15)]
    public int numberOfBullets;

    public BulletConfig bullet;
}
