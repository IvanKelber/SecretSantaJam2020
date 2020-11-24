using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    public DamageableConfig config;

    public AudioManager audioManager;
    public float currentHealth;
    public AudioSource audioSource;

    [SerializeField]
    HealthBar healthBar;

    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        currentHealth = config.maxHealth;
        healthBar.SetMaxHealth(config.maxHealth);
    }

    public virtual void TakeDamage(float damage) {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        if(currentHealth <= 0) {
            currentHealth = 0;
            Die();
        }
    }

    protected virtual void Die() {
        Destroy(this.gameObject);
    }
}
