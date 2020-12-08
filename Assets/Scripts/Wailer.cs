using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class Wailer : AIEntity
{
    public float nextWaypointDst = .01f;

    [SerializeField]
    Gun gun;

    Player nearbyPlayer;
    float timeSinceLastAttack = 0;

    int currentWaypoint = 0;
    Seeker seeker;
    Path path;
    AIMovement movement;

    List<Vector3> circularWaypoints = new List<Vector3>();

    float waypointDistanceThreshold = .2f;

    float circularPrecision = 10;
    int currentCircularWaypoint = 0;

    public override void Start() {
        base.Start();
        seeker = GetComponent<Seeker>();
        movement = GetComponent<AIMovement>();
        gun.config = AIconfig.gunConfig;

        InvokeRepeating("GetPath",.1f,1f);
    }

    public override void Update() {
        base.Update();        
    }

    List<Vector3> CalculateWaypoints(Vector3 playerPosition, float radius) {
        Debug.Log("Recalculating Circular waypoints");
        List<Vector3> newWaypoints = new List<Vector3>();
        float direction = 1;//Mathf.Sign(Random.Range(0f,1f) - .5f);
        float step = direction * 360/circularPrecision;
        float currentAngle = Vector3.SignedAngle(playerPosition, transform.position,  Vector3.forward);
        for(int i = 1; i <= circularPrecision; i++) {
            float angle = currentAngle + step * i;
            Vector3 waypoint = playerPosition + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0) * radius;
            newWaypoints.Add(waypoint);
        }
        return newWaypoints;
    }

    void MoveTowards(int waypoint) {
        Vector3 destination = circularWaypoints[waypoint];
        Vector3 direction = destination - transform.position;
        float distance = direction.magnitude;
        if(distance < waypointDistanceThreshold) {
            currentCircularWaypoint++;
            return;
        }
        Vector3 velocity = direction.normalized * AIconfig.movementSpeed;
        movement.Move(new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime);
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

    void GetPath() {
        if(nearbyPlayer != null) {
            float distance = Vector3.Distance(transform.position, nearbyPlayer.transform.position);
            if(distance <= AIconfig.minAttackDistance) {
                if(currentCircularWaypoint >= circularWaypoints.Count) {
                    circularWaypoints = CalculateWaypoints(nearbyPlayer.transform.position, distance);
                    currentCircularWaypoint = 0;
                }
                CircularPathAroundPlayer();
            } else { 
                DirectPathToPlayer();
            }
        }
    }

    void DirectPathToPlayer() {
        seeker.StartPath(transform.position, nearbyPlayer.transform.position, OnCompletePath);
    }
    void CircularPathAroundPlayer() {
        Debug.Log("A* towards " + circularWaypoints[currentCircularWaypoint]);
        Debug.Log("circularWaypoint: " + currentCircularWaypoint);
        seeker.StartPath(transform.position, circularWaypoints[currentCircularWaypoint], OnCompletePath);
    }

    void UpdateMovement() {
        if(path == null) {
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count) {
            Debug.Log("Reached the end of the path");
            currentCircularWaypoint++;
            return;
        }
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = direction * AIconfig.movementSpeed;

        movement.Move(new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime);

        if(Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) <= nextWaypointDst) {
            currentWaypoint++;
        }
        if(Vector3.Distance(transform.position, circularWaypoints[currentCircularWaypoint]) <= waypointDistanceThreshold) {
            Debug.Log("Updatr current circular waypoint: " + currentCircularWaypoint);
            currentCircularWaypoint++;
        }
    }

    void OnCompletePath(Path p) {
        if(!p.error) {
            if(nearbyPlayer != null) {
                GraphNode playerNode = AstarPath.active.GetNearest(nearbyPlayer.transform.position).node;
                if(PathUtilities.IsPathPossible(p.path[0], playerNode)) {
                    path = p;
                    currentWaypoint = 0;
                    Debug.Log("Path complete");
                } else {
                    Debug.Log("Path is null");
                    path = null;
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


        if(timeSinceLastAttack >= AIconfig.gunConfig.fireRate) {
            gun.Shoot(nearbyPlayer.transform.position);
            timeSinceLastAttack = 0;
        }
        timeSinceLastAttack += Time.deltaTime;
    }

    public override void TakeDamage(float damage, Vector3 knockback) {
        movement.Force(knockback);
        base.TakeDamage(damage, knockback);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AIconfig.minAttackDistance);
        if(circularWaypoints.Count > 0) {
            for(int i = 0; i < circularWaypoints.Count; i++) {
                if(i == currentCircularWaypoint) {
                    Gizmos.color = Color.green;
                } else {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawWireSphere(circularWaypoints[i], waypointDistanceThreshold);
            }
        }
    }
}
