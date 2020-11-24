using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : AIEntity
{
    [SerializeField]
    Gun gun;

    Player nearbyPlayer;
    float timeSinceLastAttack = 0;

    public override void Start() {
        base.Start();
        gun.config = AIconfig.gunConfig;
    }

    public override void Update() {
        if(nearbyPlayer != null) {
            transform.localScale = new Vector3(Mathf.Sign(transform.position.x - nearbyPlayer.transform.position.x), 
                                               transform.localScale.y, 
                                               transform.localScale.z);
        }
        base.Update();
    }

    protected override bool DetectPlayer() {
        Collider2D player = Physics2D.OverlapCircle(transform.position, AIconfig.detectionRadius, playerMask);
        if(player != null) {
            nearbyPlayer = player.gameObject.GetComponent<Player>();
            return true;
        }
        return false;
    }

    protected override void Search() {
        Vector3 directionToPlayer = (nearbyPlayer.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, AIconfig.maxAttackDistance, collisionMask);
        if(hit) {
            Damageable damageable = hit.collider.gameObject.GetComponent<Damageable>();
            if(damageable != null) {
                currentState = AIState.Attacking;
            } else {
            }
        } else {
            transform.position += AIconfig.movementSpeed * directionToPlayer * Time.deltaTime;
        }
        if(Vector3.Distance(nearbyPlayer.transform.position, transform.position) > AIconfig.detectionRadius) {
            currentState = AIState.Wandering;
            nearbyPlayer = null;
        }
    }

    protected override void Attack() {
        Vector3 directionToPlayer = (nearbyPlayer.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, AIconfig.maxAttackDistance * 2, collisionMask);
        if(hit) {
            if(hit.distance > AIconfig.maxAttackDistance) {
                currentState = AIState.Searching;
                return;
            } else if(hit.distance > AIconfig.minAttackDistance) {
                transform.position += AIconfig.movementSpeed * directionToPlayer * Time.deltaTime;
            } else {
                // move along a circle at that distance
                //TODO:
            }
        }


        if(timeSinceLastAttack >= AIconfig.gunConfig.fireRate) {
            gun.Shoot(nearbyPlayer.transform.position);
            timeSinceLastAttack = 0;
        }
        timeSinceLastAttack += Time.deltaTime;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AIconfig.detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AIconfig.maxAttackDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AIconfig.minAttackDistance);

    }
}
