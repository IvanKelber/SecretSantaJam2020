using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    LayerMask collisionMask;

    [SerializeField]
    float lifeTime = 3;

    AudioSource audioSource;
    Vector3 direction;
    float birthTime;
    CinemachineImpulseSource impulseSource;

    CircleCollider2D collider;
    bool destroying = false;
    void Start() {
        birthTime = Time.time;
        audioSource = gameObject.AddComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        collider = GetComponent<CircleCollider2D>();
        impulseSource.GenerateImpulse(direction);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if(CheckCollision()) {
            if(!destroying)
                StartCoroutine(Collision());
        }
        if(Time.time - birthTime >= lifeTime) {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(Vector3 direction) {
        this.direction = direction;
    }

    public bool CheckCollision() {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), collider.radius,
                                                            collisionMask);
        Debug.Log(collisions.Length);
        return collisions.Length > 0;
    }

    IEnumerator Collision() {
        destroying = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(0,0,0,0);
        yield return audioManager.PlayAndWait("BulletCollision", audioSource);
        Destroy(this.gameObject);
    }
}
