using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntity : MonoBehaviour, IDamageable
{

    public enum AIState {
        Wandering,
        Searching,
        Attacking 
    }
    public AIConfig AIconfig;
    
    public GameObject goldPrefab;
    public float currentHealth;
    public AudioManager audioManager;
    public AudioSource audioSource;


    [SerializeField]
    protected LayerMask playerMask, wallMask;
    [SerializeField]
    protected HealthBar healthBar;

    protected LayerMask collisionMask;
    
    protected AIState currentState = AIState.Wandering;

    public virtual void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        currentHealth = AIconfig.maxHealth;
        healthBar.SetMaxHealth(AIconfig.maxHealth);

        collisionMask = playerMask | wallMask;
    }

    public virtual void Update()
    {
        switch(currentState) {
            case AIState.Wandering:
                Wander();
                break;
            case AIState.Searching:
                Search();
                break;
            case AIState.Attacking:
                Attack();
                break;
        }
    }

    protected virtual bool DetectPlayer() {
        return false;
    }

    protected virtual void Wander() {
        if(DetectPlayer()) {
            currentState = AIState.Searching;
        };
    }

    protected virtual void Search() {

    }

    protected virtual void Attack() {

    }

    public virtual void TakeDamage(float damage) {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth,0,AIconfig.maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
        if(currentHealth == 0) {
            Die();
        }
    }

    public virtual void TakeDamage(float damage, Vector3 knockback) {
        TakeDamage(damage);
    }

    protected virtual void Die() {
        DropGold();
        Destroy(this.gameObject);
    }




    void DropGold() {
        Instantiate(goldPrefab, transform.position, Quaternion.identity);
    }
}
