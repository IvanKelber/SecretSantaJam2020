using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    AudioManager audioManager;

    BulletConfig config;
    PlayerValues playerValues;

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
    int bounces = 0;
    bool justBounced = false;
    bool enemyHit = false;

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
        bounces = config.bulletBounces;
        transform.localScale = config.bulletScale;
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
        justBounced = false;
        enemyHit = false;
    }

    public void SetDirection(Vector3 direction) {
        this.direction = direction;
    }
    public void SetConfig(BulletConfig config) {
        this.config = config;
    }

    //Should be called before Start directly after instantiating
    public void SetPlayerValues(PlayerValues values) {
        this.playerValues = values;
        config.bulletSpeed = playerValues.projectileSpeed;
        config.bulletDamage = playerValues.projectileDamage;
        config.knockbackOnHit = playerValues.knockbackOnHit;
        config.bulletRange = playerValues.projectileRange;
        config.bulletBounces = playerValues.projectileBounces;
    }

    public Collider2D[] CheckCollision() {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), collider.radius,
                                                            config.damagingLayer);
        return collisions;
    }

    IEnumerator Collision(Collider2D[] collisions) {
        Collider2D collider = collisions[0]; // Get only the first collision unless piercing
        if(DamageObject(collider)) {
            if(enemyHit) {
                destroying = true;
                Destroy(this.gameObject);
            }
        } else if (bounces > 0) {
            // Projectile hit a wall but has bounces left
            Bounce(collider);
            bounces--;
        } else if(!justBounced) {
            destroying = true;
            impulseSource.GenerateImpulse(direction);
            renderer.color = new Color(0,0,0,0);
            yield return audioManager.PlayAndWait(config.collisionSound, audioSource);
            Destroy(this.gameObject);
        }
        
    }

    public bool DamageObject(Collider2D collider) {
        IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
        if(damageable != null) {
            Vector3 knockback = (collider.transform.position - this.transform.position).normalized * config.knockbackOnHit;
            enemyHit = damageable.TakeDamage(config.bulletDamage, knockback);
            return true;
        }
        // Hit something that we can't damage (i.e. a wall)
        return false;
    }

    public void Bounce(Collider2D collider) {
        float distance = Vector3.Distance(collider.transform.position, transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, this.direction, distance, 1 << collider.gameObject.layer);
        if(hit) {
            Vector2 normal = hit.normal;
            this.direction = Vector3.Reflect(this.direction, new Vector3(normal.x, normal.y, 0));
            velocity = this.direction.normalized * config.bulletSpeed;  
            justBounced = true;
        }
    }


}
