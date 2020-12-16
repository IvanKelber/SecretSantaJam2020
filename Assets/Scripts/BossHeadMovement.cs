using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeadMovement : MonoBehaviour, IDamageable
{

    public Boss boss;
    public float radius = 10;
    public float angularSpeed = 15; //degrees per second
    float angle;
    void Start()
    {
        angle = Random.Range(0,360);
    }

    void Update()
    {
        Vector3 center = boss.transform.position;
        float distance = radius + Mathf.Sin(Mathf.Deg2Rad * angle * 2) * 2;
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), -1);
        transform.position = center + direction * radius;
        angle += angularSpeed * Time.deltaTime;
    }

    public bool TakeDamage(float damage) {
        return boss.TakeDamage(damage);
    }

    public bool TakeDamage(float damage, Vector3 knockback) {
        return boss.TakeDamage(damage, knockback);
    }
}
