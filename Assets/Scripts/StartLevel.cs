using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class StartLevel : Interactable
{

    public GameObject player;
    public Dungeon dungeon;
    public Transform playerDeathSpawn;
    public Camera camera;
    public GameEvent levelComplete;

    private int currentLevel = 0;
    private bool playerDead = false;

    public void OnLevelComplete() {
        NextLevel(++currentLevel);
    }

    public void OnPlayerDeath() {
        if(playerDead) {
            dungeon.DestroyCurrentLevel();
            player.transform.position = playerDeathSpawn.position;
            UpdateCamera(player.transform.position);
            playerDead = false;
        }
    }

    public void NextLevel(int level) {
        dungeon.GenerateLevel(level);
        player.transform.position = dungeon.GetStartRoom();
        UpdateCamera(player.transform.position);
    }

    public override void OnInteract() {
        levelComplete.Raise();
    }

    void UpdateCamera(Vector3 destination) {
        camera.transform.position = new Vector3(destination.x, destination.y, camera.transform.position.z);

    }

    public void SetPlayerDead() {
        playerDead = true;
    }


}
