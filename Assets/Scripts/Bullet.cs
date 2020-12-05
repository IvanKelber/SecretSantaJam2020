﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    AudioManager audioManager;

    BulletConfig config;

    AudioSource audioSource;
    Vector3 direction;
    Vector3 birthPosition;
    float speed;
    CircleCollider2D collider;
    CinemachineImpulseSource impulseSource;
    SpriteRenderer renderer;
    bool destroying = false;
    Vector2 velocity;
    Vector2 gravity;

    void Start() {
        birthPosition = transform.position;
        audioSource = gameObject.AddComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        collider = GetComponent<CircleCollider2D>();
        renderer = GetComponent<SpriteRenderer>();

        impulseSource.m_ImpulseDefinition = config.collisionImpulse;
        if(config.sprite != null) {
            renderer.sprite = config.sprite;
        }
        velocity = direction.normalized * config.bulletSpeed;
        gravity = (Vector2)(Quaternion.Euler(0,0,config.gravityDirection) * Vector3.right).normalized * config.gravityMagnitude;
    }

    void Update()
    {
        if(StaticUserControls.paused) {
            return;
        }
        velocity += gravity * Time.deltaTime;
        transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
        Collider2D[] collisions = CheckCollision();
        if(collisions.Length > 0) {
            if(!destroying)
                StartCoroutine(Collision(collisions));
        }
        if(!destroying && Vector3.Distance(transform.position, birthPosition) >= config.bulletRange) {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(Vector3 direction) {
        this.direction = direction;
    }
    public void SetSpeed(float speed) {
        config.bulletSpeed = speed;
    }

    public void SetConfig(BulletConfig config) {
        this.config = config;
    }

    public void SetDamage(float damage) {
        this.config.bulletDamage = damage;
    }

    public void SetKnockbackOnHit(float knockback) {
        this.config.knockbackOnHit = knockback;
    }

    public void SetRange(float range) {
        this.config.bulletRange = range;
    }

    public Collider2D[] CheckCollision() {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), collider.radius,
                                                            config.damagingLayer);
        return collisions;
    }

    IEnumerator Collision(Collider2D[] collisions) {
        destroying = true;
        renderer.color = new Color(0,0,0,0);
        impulseSource.GenerateImpulse(direction);
        foreach(Collider2D collider in collisions) {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if(damageable != null) {
                Vector3 knockback = (collider.transform.position - this.transform.position).normalized * config.knockbackOnHit;
                damageable.TakeDamage(config.bulletDamage, knockback);
            }
        }
        yield return audioManager.PlayAndWait(config.collisionSound, audioSource);
        Destroy(this.gameObject);
    }


}
