using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
[CreateAssetMenu(menuName="Configs/Bullet")]
public class BulletConfig : ScriptableObject
{
    [Range(0f,200)]
    public float bulletRange = 1;

    [Range(1,100)]
    public float bulletDamage = 5;

    public CinemachineImpulseDefinition collisionImpulse;

    public string collisionSound;

    [Range(1,100)]
    public float bulletSpeed = 20;

    public LayerMask damagingLayer;

    public Sprite sprite;

    [Range(-180,180)]
    public float gravityDirection = -90; //initially straight down

    public float gravityMagnitude = 0;
    
    public bool independentOfAim = false;

    public float knockbackOnHit = 1;

    public int bulletBounces = 0;

    public Vector3 bulletScale = Vector3.one;

}
