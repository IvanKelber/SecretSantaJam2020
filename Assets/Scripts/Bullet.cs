using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;

    [SerializeField]
    float lifeTime = 3;

    Vector3 direction;
    float birthTime;

    void Start() {
        birthTime = Time.time;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if(CheckCollision()) {
            Destroy(this.gameObject);
        }
        if(Time.time - birthTime >= lifeTime) {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(Vector3 direction) {
        this.direction = direction;
    }

    public bool CheckCollision() {
        return false;
    }
}
