using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : Interactable
{
    [SerializeField]
    EnemySpawner enemySpawner;
    RoomConfig config;
    BoxCollider2D roomBoundaries;

    public void SetConfig(RoomConfig config) {
        this.config = config;
    }

    public override void OnEnterTrigger(Collider2D collider) {
        if(enemySpawner != null) {
            enemySpawner.SetRoomConfig(config);
            enemySpawner.StartSpawning();
            LockRoom();
        }
    }

    public void LockRoom() {
        Debug.Log("Room is locked");
    }
}
