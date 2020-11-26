using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{

    public GameObject player;
    public Dungeon dungeon;
    public LayerMask playerMask;

    private int currentLevel = 0;
    private bool playerPresent = false;

    void Update() {
        if(playerPresent && Input.GetKeyDown(KeyCode.E)) {
            NextLevel(currentLevel);
        }
    }

    public void OnLevelComplete() {
        NextLevel(++currentLevel);
    }

    public void NextLevel(int level) {
        dungeon.GenerateLevel(level);
        player.transform.position = dungeon.GetStartRoom();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            playerPresent = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            playerPresent = false;
        }
    }
}
