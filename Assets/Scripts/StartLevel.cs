using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using Cinemachine;

public class StartLevel : Interactable
{

    public GameObject player;
    public Dungeon dungeon;
    public Transform playerDeathSpawn;
    public CinemachineVirtualCamera vcam;
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
            currentLevel = 0;
            player.transform.position = playerDeathSpawn.position;
            UpdateCamera(player.transform);
            playerDead = false;
        }
    }

    public void NextLevel(int level) {
        dungeon.GenerateLevel(level);
        player.transform.position = dungeon.GetStartRoom() + Vector3.forward * -1f;
        UpdateCamera(player.transform);
    }

    public override void OnInteract() {
        levelComplete.Raise();
    }

    void UpdateCamera(Transform pt) {
        vcam.gameObject.SetActive(false);
        camera.transform.position = new Vector3(pt.position.x, pt.position.y, camera.transform.position.z);
        StartCoroutine(ReactivateCamera());
        // vcam.gameObject.SetActive(true);

    }

    public IEnumerator ReactivateCamera() {
        yield return new WaitForSeconds(.1f);
        vcam.gameObject.SetActive(true);
    }

    public void SetPlayerDead() {
        playerDead = true;
    }


}
