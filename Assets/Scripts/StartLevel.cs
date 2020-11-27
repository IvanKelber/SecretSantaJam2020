using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{

    public GameObject player;
    public Dungeon dungeon;
    public LayerMask playerMask;
    public Transform playerDeathSpawn;

    public GameObject interact;

    private int currentLevel = 0;
    private bool playerPresent = false;
    private Transform playerTransform;

    void Update() {
        if(playerPresent) {
            if(Input.GetKeyDown(KeyCode.E)) {
                NextLevel(currentLevel);
            }
        }
    }

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

    void OnTriggerEnter2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            playerPresent = true;
            playerTransform= collision.transform;
            interact.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            playerPresent = false;
            playerTransform = null;
            interact.SetActive(false);
        }
    }
}
