using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerGun : MonoBehaviour
{
    public AudioManager audioManager;
    public GameObject projectilePrefab;

    [SerializeField]
    PlayerValues playerValues;

    AudioSource audioSource;
    CinemachineImpulseSource impulseSource;
    SpriteRenderer spriteRenderer;
    bool shooting = false;

    void Awake() {
        audioSource = gameObject.AddComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(playerValues != null) {
            spriteRenderer.sprite = playerValues.gunSprite;
        }
        
        impulseSource.m_ImpulseDefinition = playerValues.fireImpulse;

    }

    public Vector3 GetCenter(Vector3 direction) {
        return (direction - transform.position).normalized;
    }

    public void Shoot(Vector3 direction) {
        if(!shooting) {
            StartCoroutine(DoShoot(direction));
        }
        
    }

    IEnumerator DoShoot(Vector3 direction) {
        shooting = true;
        float angleStep = playerValues.projectileSpread * 2;
        for(int i = 0; i < playerValues.numberOfProjectilesPerShot; i++) {
            ShotFX();

            Bullet bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.SetConfig(playerValues.projectile);
            bullet.SetSpeed(playerValues.projectileSpeed);
            Vector3 noisyDirection = GetCenter(direction);

            noisyDirection = Quaternion.Euler(0,0, - playerValues.projectileSpread + (angleStep * i)) * noisyDirection;
            noisyDirection = Quaternion.Euler(0,0, Random.Range(-playerValues.projectileSpreadNoise, playerValues.projectileSpreadNoise)) * noisyDirection;
            bullet.SetDirection(noisyDirection);
            if(playerValues.subsequentProjectileDelay > 0) {
                yield return new WaitForSeconds(playerValues.subsequentProjectileDelay);
            }
        }
        shooting = false;
    }

    public void ShotFX() {
        impulseSource.GenerateImpulse();
        audioManager.Play("BulletBirth", audioSource);
    }

    void OnDrawGizmos() {
        if(playerValues != null) {
            Gizmos.color = Color.white;
            // Vector3 center = Quaternion.Euler(0,0,playerValues.centerAngle) * Vector3.right;
            // Gizmos.DrawRay(transform.position, center * 2);
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawRay(transform.position, Quaternion.Euler(0,0,-playerValues.projectileSpread) *center * 2);
            // Gizmos.DrawRay(transform.position, Quaternion.Euler(0,0,playerValues.projectileSpread) *center * 2);
        }
    }
}
