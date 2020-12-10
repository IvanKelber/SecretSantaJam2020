using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
[CreateAssetMenu(menuName="Configs/Gun")]
public class GunConfig : ScriptableObject
{
    [Range(0.05f, 10f)]
    public float fireRate = 1;

    public CinemachineImpulseDefinition fireImpulse;
    public string fireSound;

    [Range(0,100)]
    public float knockback = 0;

    [Range(0,359)]
    public float spreadNoise = 0;

    [Range(0, 359)]
    public float angleBetweenBullets = 0;

    [Range(0, 359)]
    public float centerAngle = 0;

    [Range(1,100)]
    public int numberOfBullets = 1;

    public BulletConfig bullet;

    [Range(0, 2)]
    public float timeBetweenShots = 0;

    public bool independentOfAim = false;

    public bool changeDirectionOverTime = false;

    public AnimationCurve xDirectionalChange;
    public AnimationCurve yDirectionalChange;

    public Sprite gunSprite;
}
