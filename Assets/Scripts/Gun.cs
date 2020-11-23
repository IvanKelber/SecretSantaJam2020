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


    public void Shoot(Vector3 direction) {
        for(int i = 0; i < config.numberOfBullets; i++) {
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.SetConfig(config.bullet);
            Vector3 noisyDirection = (direction - transform.position).normalized;
            noisyDirection = Quaternion.Euler(0,0, Random.Range(-config.spreadNoise, config.spreadNoise)) * noisyDirection;
            bullet.SetDirection(noisyDirection);
        }
        impulseSource.GenerateImpulse();
        audioManager.Play(config.fireSound, audioSource);
    
    }
    

}
