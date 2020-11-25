using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{

    public GameObject player;
    public Dungeon dungeon;

    void Start()
    {
        dungeon.GenerateGrid();
        player.transform.position = dungeon.GetStartRoom();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Start();
        }
    }
}
