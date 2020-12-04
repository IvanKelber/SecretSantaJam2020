using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    public DamageableConfig config;

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
        currentHealth = Mathf.Clamp(currentHealth,0,config.maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
        if(currentHealth == 0) {
            Die();
        }
    }

    public virtual void Heal(float heal) {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth,0,config.maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
    }

    protected virtual void Die() {
        Destroy(this.gameObject);
    }
}
