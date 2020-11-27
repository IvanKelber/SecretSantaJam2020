using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Gun : MonoBehaviour
{

    public GunConfig config;
    public AudioManager audioManager;
    public GameObject bulletPrefab;
    AudioSource audioSource;
    CinemachineImpulseSource impulseSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        impulseSource.m_ImpulseDefinition = config.fireImpulse;

    }

    public Vector3 GetCenter(Vector3 direction) {
        if(config.bullet.independentOfAim) {
            return Quaternion.Euler(0,0,config.centerAngle) * Vector3.right;
        }
        return (direction - transform.position).normalized;
    }

    public void Shoot(Vector3 direction) {
        float angleStep = config.angleBetweenBullets * 2;
        for(int i = 0; i < config.numberOfBullets; i++) {
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.SetConfig(config.bullet);
            Vector3 noisyDirection = GetCenter(direction);

            noisyDirection = Quaternion.Euler(0,0, - config.angleBetweenBullets + (angleStep * i)) * noisyDirection;
            Debug.Log("Direction: " + noisyDirection);
            noisyDirection = Quaternion.Euler(0,0, Random.Range(-config.spreadNoise, config.spreadNoise)) * noisyDirection;
            bullet.SetDirection(noisyDirection);
        }
        impulseSource.GenerateImpulse();
        audioManager.Play(config.fireSound, audioSource);
    
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Vector3 center = Quaternion.Euler(0,0,config.centerAngle) * Vector3.right;
        Gizmos.DrawRay(transform.position, center * 2);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0,0,-config.angleBetweenBullets) *center * 2);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0,0,config.angleBetweenBullets) *center * 2);
    }
    

}
