using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    public DamageableConfig config;

    public AudioManager audioManager;
    public float currentHealth;
    public AudioSource audioSource;
    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        currentHealth = config.maxHealth;
    }

    public virtual void TakeDamage(float damage) {
        currentHealth -= damage;
        if(currentHealth <= 0) {
            currentHealth = 0;
            Die();
        }
    }

    protected virtual void Die() {
        Destroy(this.gameObject);
    }
}
