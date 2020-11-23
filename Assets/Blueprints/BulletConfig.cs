using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
[CreateAssetMenu(menuName="BulletConfig")]
public class BulletConfig : ScriptableObject
{
    [Range(.5f,5)]
    public float lifetime;

    public CinemachineImpulseDefinition collisionImpulse;

    public string collisionSound;

    [Range(1,100)]
    public float bulletSpeed;
}
