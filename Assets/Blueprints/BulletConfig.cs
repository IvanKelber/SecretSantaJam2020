using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
[CreateAssetMenu(menuName="Configs/Bullet")]
public class BulletConfig : ScriptableObject
{
    [Range(.5f,5)]
    public float lifetime = 1;

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

}
