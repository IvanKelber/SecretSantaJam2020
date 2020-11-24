using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntity : Damageable
{

    public enum AIState {
        Wandering,
        Searching,
        Attacking 
    }
    
    protected AIState currentState = AIState.Wandering;

    [SerializeField]
    protected AIConfig AIconfig;

    [SerializeField]
    protected LayerMask playerMask, wallMask;

    protected LayerMask collisionMask;
    

    public virtual void Start()
    {
        base.Start();
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
}
