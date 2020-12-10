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
    bool roomEntered = false;
    int difficulty = 1;

    public void SetConfig(RoomConfig config) {
        this.config = config;
    }

    public void SetDifficulty(int difficulty) {
        this.difficulty = difficulty;
    }

    public override void Start() {
        base.Start();
        roomComplete = enemySpawner == null;
    }

    public override void OnEnterTrigger(Collider2D collider) {
        if(!roomEntered && config.spawnEnemies && !roomComplete) {
            roomEntered = true;
            enemySpawner.SetRoomConfig(config);
            enemySpawner.SetDifficulty(difficulty);
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
