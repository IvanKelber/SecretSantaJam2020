using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class RoomBehavior : Interactable
{
    [SerializeField]
    EnemySpawner enemySpawner;

    [SerializeField]
    GameObject rewardCollectionPrefab;

    RoomConfig config;
    BoxCollider2D roomBoundaries;

    bool roomComplete = false;

    public void SetConfig(RoomConfig config) {
        this.config = config;
    }

    public override void Start() {
        base.Start();
        roomComplete = enemySpawner == null;
    }

    public override void OnEnterTrigger(Collider2D collider) {
        if(config.spawnEnemies && !roomComplete) {
            enemySpawner.SetRoomConfig(config);
            enemySpawner.StartSpawning();
            LockRoom();
        }
    }

    public void LockRoom() {
        config.lockDoors.Raise(true);
    }

    public void OnRoomComplete() {
        //spawn rewards
        if(playerPresent) {
            roomComplete = true;
            Instantiate(rewardCollectionPrefab, transform.position + Vector3.down * 5, Quaternion.identity);
        }

        
    }
}
