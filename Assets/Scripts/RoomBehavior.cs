using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class RoomBehavior : Interactable
{
    [SerializeField]
    EnemySpawner enemySpawner;
    RoomConfig config;
    BoxCollider2D roomBoundaries;
    Room room;
    [SerializeField]

    public void SetConfig(RoomConfig config) {
        this.config = config;
    }

    public void SetRoom(Room room) {
        this.room = room;
    }

    public override void OnEnterTrigger(Collider2D collider) {
        if(config.spawnEnemies && enemySpawner != null) {
            enemySpawner.SetRoomConfig(config);
            enemySpawner.StartSpawning();
            LockRoom();
        }
    }

    public void LockRoom() {
        config.lockDoors.Raise(true);
        // room.LockDoors(true);
    }
}
