using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    LayerMask collisionMask;

    BulletConfig config;

    AudioSource audioSource;
    Vector3 direction;
    float birthTime;
    CircleCollider2D collider;
    CinemachineImpulseSource impulseSource;
    bool destroying = false;
    void Start() {
        birthTime = Time.time;
        audioSource = gameObject.AddComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        collider = GetComponent<CircleCollider2D>();
        impulseSource.m_ImpulseDefinition = config.collisionImpulse;
    }

    void Update()
    {
        transform.position += direction * config.bulletSpeed * Time.deltaTime;
        if(CheckCollision()) {
            if(!destroying)
                StartCoroutine(Collision());
        }
        if(Time.time - birthTime >= config.lifetime) {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(Vector3 direction) {
        this.direction = direction;
    }

    public void SetConfig(BulletConfig config) {
        this.config = config;
    }

    public bool CheckCollision() {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), collider.radius,
                                                            collisionMask);
        return collisions.Length > 0;
    }

    IEnumerator Collision() {
        destroying = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(0,0,0,0);
        impulseSource.GenerateImpulse(direction);

        yield return audioManager.PlayAndWait(config.collisionSound, audioSource);
        //Destroy(this.gameObject);
    }
}
