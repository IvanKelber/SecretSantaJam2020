using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : AIEntity
{
    [SerializeField]
    Gun gun;

    PlayerMovement nearbyPlayer;
    float timeSinceLastAttack = 0;

    public override void Start() {
        base.Start();
        gun.config = AIconfig.gunConfig;
    }

    protected override bool DetectPlayer() {
        Collider2D player = Physics2D.OverlapCircle(transform.position, AIconfig.detectionRadius, playerMask);
        if(player != null) {
            nearbyPlayer = player.gameObject.GetComponent<PlayerMovement>();
            return true;
        }
        return false;
    }

    protected override void Search() {
        Vector3 directionToPlayer = (nearbyPlayer.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, AIconfig.maxAttackDistance, collisionMask);
        if(hit) {
            PlayerMovement player = hit.collider.gameObject.GetComponent<PlayerMovement>();
            if(player != null) {
                currentState = AIState.Attacking;
            } else {
                Debug.Log("There's a wall in the way of the player");
            }
        } else {
            transform.position += AIconfig.movementSpeed * directionToPlayer;
        }
    }

    protected override void Attack() {
        if(timeSinceLastAttack >= AIconfig.gunConfig.fireRate) {
            gun.Shoot(nearbyPlayer.transform.position);
            timeSinceLastAttack = 0;
        }
        timeSinceLastAttack += Time.deltaTime;
    }
}
