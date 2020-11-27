using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : Interactable
{

    public GameObject player;
    public Dungeon dungeon;
    public Transform playerDeathSpawn;

    private int currentLevel = 0;

    public void OnLevelComplete() {
        NextLevel(++currentLevel);
    }

    public void OnPlayerDeath() {
        dungeon.DestroyCurrentLevel();
        player.transform.position = playerDeathSpawn.position;
    }

    public void NextLevel(int level) {
        dungeon.GenerateLevel(level);
        player.transform.position = dungeon.GetStartRoom();
    }

    public override void OnInteract() {
        NextLevel(currentLevel);
    }
}
