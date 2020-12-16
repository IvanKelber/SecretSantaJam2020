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

    [SerializeField]
    protected Renderer renderer;

    [SerializeField,
    Range(0,10)]
    protected int flashFrames = 2;
    bool flashing = false;


    protected LayerMask collisionMask;
    
    protected AIState currentState = AIState.Wandering;
    protected int difficulty = 1;

    public virtual void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        AIconfig.maxHealth = AIconfig.baseMaxHealth * difficulty;

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

    public virtual bool TakeDamage(float damage) {
        if(!flashing) {
            StartCoroutine(Flash());
        }
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth,0,AIconfig.maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
        if(currentHealth == 0) {
            Die();
        }
        return true;
    }

    public virtual bool TakeDamage(float damage, Vector3 knockback) {
        return TakeDamage(damage);
    }

    protected IEnumerator Flash() {
        flashing = true;
        Color initialColor = renderer.material.color;
        renderer.material.color = Color.white;
        for(int i = 0; i < flashFrames; i++) {
            yield return null;
        }
        renderer.material.color = initialColor;
        flashing = false;
    }


    public void SetDifficulty(int difficulty) {
        this.difficulty = difficulty;
    }

    protected virtual void Die() {
        DropGold();
        Destroy(this.gameObject);
    }




    void DropGold() {
        Instantiate(goldPrefab, transform.position, Quaternion.identity);
    }
}
