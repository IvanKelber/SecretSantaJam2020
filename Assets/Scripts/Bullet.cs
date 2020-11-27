using System.Collections;
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
    float birthTime;
    CircleCollider2D collider;
    CinemachineImpulseSource impulseSource;
    SpriteRenderer renderer;
    bool destroying = false;

    Vector2 velocity;
    Vector2 gravity;

    void Start() {
        birthTime = Time.time;
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
        velocity += gravity * Time.deltaTime;
        transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
        Collider2D[] collisions = CheckCollision();
        if(collisions.Length > 0) {
            if(!destroying)
                StartCoroutine(Collision(collisions));
        }
        if(!destroying && Time.time - birthTime >= config.lifetime) {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(Vector3 direction) {
        this.direction = direction;
    }

    public void SetConfig(BulletConfig config) {
        this.config = config;
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
            Damageable damageable = collider.gameObject.GetComponent<Damageable>();
            if(damageable != null) {
                damageable.TakeDamage(config.bulletDamage);
            }
        }
        yield return audioManager.PlayAndWait(config.collisionSound, audioSource);
        Destroy(this.gameObject);
    }
}
