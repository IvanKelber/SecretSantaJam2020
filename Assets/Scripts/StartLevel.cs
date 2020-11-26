using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{

    public GameObject player;
    public Dungeon dungeon;

    private int currentLevel = 0;
    void Start()
    {
        NextLevel(currentLevel);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Start();
        }
    }

    public void OnLevelComplete() {
        NextLevel(++currentLevel);
    }

    public void NextLevel(int level) {
        dungeon.GenerateLevel(level);
        player.transform.position = dungeon.GetStartRoom();
    }
}
