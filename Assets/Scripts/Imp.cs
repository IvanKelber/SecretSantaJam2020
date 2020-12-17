using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class Imp : AIEntity
{
    public float nextWaypointDst = 1;

    [SerializeField]
    protected Gun gun;

    [SerializeField]
    GameObject mouth;

    [SerializeField]
    GameObject body;

    protected Player nearbyPlayer;
    protected float timeSinceLastAttack = 0;
 
    protected int currentWaypoint = 0;
    protected Seeker seeker;
    protected Path path;
    protected AIMovement movement;

    protected Vector3 initialScale;

    bool playerInLOS = false;
    bool attackLoop = false;
    bool shaking = false;

    public override void Start() {
        base.Start();
        initialScale = transform.localScale;
        seeker = GetComponent<Seeker>();
        movement = GetComponent<AIMovement>();
        gun.config = AIconfig.gunConfig;
        InvokeRepeating("UpdatePath", 0,.5f);
    }

    public override void Update() {
        base.Update();
    }

    void IsPlayerInLOS() {
        Vector3 directionToPlayer = (nearbyPlayer.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, AIconfig.maxAttackDistance * 2, collisionMask);
        if(hit) {
            if(hit.collider != null) {
                playerInLOS = (hit.collider.gameObject.GetComponent<Player>() != null);
            }
        } else {
            playerInLOS = false;
        }
    }

    protected override bool DetectPlayer() {
        Collider2D player = Physics2D.OverlapCircle(transform.position, AIconfig.detectionRadius, playerMask);
        if(player != null) {
            nearbyPlayer = player.gameObject.GetComponent<Player>();
            // audioManager.Play("ZombieGrunt", audioSource);
            return true;
        }
        return false;
    }

    protected override void Search() {
        UpdateMovement();
        float playerDistance = Vector3.Distance(transform.position, nearbyPlayer.transform.position);
        if(playerDistance > AIconfig.detectionRadius) {
            nearbyPlayer = null;
            currentState = AIState.Wandering;
        } else if(playerDistance <= AIconfig.maxAttackDistance) {
            currentState = AIState.Attacking;
        }
    }

    void UpdatePath() {
        if(nearbyPlayer != null) {
            IsPlayerInLOS();

            if(playerInLOS && Vector3.Distance(transform.position, nearbyPlayer.transform.position) <= AIconfig.minAttackDistance) {
                path = null;
            } else {
                seeker.StartPath(transform.position, nearbyPlayer.transform.position, OnCompletePath);
            }
        }
    }

    void UpdateMovement() {
        if(path == null) {
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count - 1) {
            return;
        }
        Vector3 direction = (path.vectorPath[currentWaypoint + 1] - transform.position).normalized;
        Vector3 velocity = direction * AIconfig.movementSpeed;

        movement.Move(new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime);

        if(Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) <= nextWaypointDst) {
            currentWaypoint++;
        }
    }

    void OnCompletePath(Path p) {
        if(!p.error) {
            if(nearbyPlayer != null) {
                GraphNode playerNode = AstarPath.active.GetNearest(nearbyPlayer.transform.position).node;
                if(PathUtilities.IsPathPossible(p.path[0], playerNode)) {
                    path = p;
                    currentWaypoint = 0;
                }
            }
        }
    }

    protected override void Attack() {
        UpdateMovement();
        Vector3 directionToPlayer = (nearbyPlayer.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, AIconfig.maxAttackDistance * 2, collisionMask);
        if(hit) {
            if(hit.distance > AIconfig.maxAttackDistance) {
                currentState = AIState.Searching;
                return;
            } else {
                IDamageable d = hit.collider.gameObject.GetComponent<IDamageable>();
                if(d == null) {
                    return;
                }
            }
        }

        if(!attackLoop) {
            StartCoroutine(DoAttack());
        }
    }

    IEnumerator DoAttack() {
        attackLoop = true;
        float mouthScale = .5f;
        Tween attackTween = mouth.transform.DOScale(mouthScale, gun.config.fireRate);
        yield return attackTween.WaitForCompletion();
        Tween scaleDown = mouth.transform.DOScale(0, .1f);
        gun.Shoot(nearbyPlayer.transform.position);
        yield return scaleDown.WaitForCompletion();
        attackLoop = false;
    }


    public override bool TakeDamage(float damage, Vector3 knockback) {
        movement.Force(knockback);
        if(!shaking) {
            StartCoroutine(Shake(knockback.normalized));
        }
        return base.TakeDamage(damage, knockback);
    }

    IEnumerator Shake(Vector3 direction) {
        shaking = true;
        yield return body.transform.DOShakeScale(.3f,direction,40,60).WaitForCompletion();
        shaking = false;

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
