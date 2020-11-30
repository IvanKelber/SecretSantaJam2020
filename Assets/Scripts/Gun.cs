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

    SpriteRenderer spriteRenderer;

    bool shooting = false;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = config.gunSprite;
        
        impulseSource.m_ImpulseDefinition = config.fireImpulse;

    }

    public void SetConfig(GunConfig config) {
        this.config = config;
        spriteRenderer.sprite = config.gunSprite;
    }

    public Vector3 GetCenter(Vector3 direction) {
        if(config.bullet.independentOfAim) {
            return Quaternion.Euler(0,0,config.centerAngle) * Vector3.right;
        }
        return (direction - transform.position).normalized;
    }

    public void Shoot(Vector3 direction) {
        if(!shooting) {
            StartCoroutine(DoShoot(direction));
        }
        
    }

    IEnumerator DoShoot(Vector3 direction) {
        Debug.Log("Shot direction: " + GetCenter(direction));
        shooting = true;
        float angleStep = config.angleBetweenBullets * 2;
        for(int i = 0; i < config.numberOfBullets; i++) {
            ShotFX();

            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.SetConfig(config.bullet);
            Vector3 noisyDirection = GetCenter(direction);

            noisyDirection = Quaternion.Euler(0,0, - config.angleBetweenBullets + (angleStep * i)) * noisyDirection;
            noisyDirection = Quaternion.Euler(0,0, Random.Range(-config.spreadNoise, config.spreadNoise)) * noisyDirection;
            bullet.SetDirection(noisyDirection);
            if(config.timeBetweenShots > 0) {
                yield return new WaitForSeconds(config.timeBetweenShots);
            }
        }
        shooting = false;
    }

    public void ShotFX() {
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
