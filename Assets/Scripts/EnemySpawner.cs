using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{


    public LayerMask playerMask;
    public float spawnDuration = 1;
    


    List<GameObject> spawnedEnemies = new List<GameObject>();

    List<Transform> spawnPoints = new List<Transform>();

    private int totalEnemiesSpawned = 0;
    private float timeSinceLastSpawn = 0;
    private bool playerEnteredRoom = false;
    private int enemiesToSpawn;

    //Config
    private int minEnemiesSpawned;
    private int maxEnemiesSpawned;
    private float timeBetweenSpawns;
    private List<GameObject> enemyPrefabs = new List<GameObject>();


    void Start() {
        timeSinceLastSpawn = timeBetweenSpawns;
        foreach(Transform child in transform) {
            if(child.gameObject.tag == "SpawnPoint") {
                spawnPoints.Add(child);
            }
        }
    }

    void Update()
    {
        if(StaticUserControls.paused || !playerEnteredRoom) {
            return;
        }
        if(timeSinceLastSpawn >= timeBetweenSpawns && totalEnemiesSpawned < enemiesToSpawn) {
            timeSinceLastSpawn = 0;
            StartCoroutine(Spawn());
        } else if(totalEnemiesSpawned == enemiesToSpawn) {
            //The room has been completed
            Debug.Log("Room is completed");
        }
        timeSinceLastSpawn += Time.deltaTime;
    }

    public void SetRoomConfig(RoomConfig config) {
        minEnemiesSpawned = config.minNumberOfEnemies;
        maxEnemiesSpawned = config.maxNumberOfEnemies;
        timeBetweenSpawns = config.spawnRate;
        enemyPrefabs = config.enemiesToSpawn;

    }

    public IEnumerator Spawn() {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        GameObject spawnIndicator = Instantiate(enemyPrefab.GetComponent<AIEntity>().AIconfig.spawnIndicator, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnDuration);
        Destroy(spawnIndicator);
        yield return null;        
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemy.transform.parent = spawnPoint;
        enemy.transform.localScale = Vector3.one;
        totalEnemiesSpawned++;
    }


    public void ClearEnemies() {
        foreach(GameObject enemy in spawnedEnemies) {
            Destroy(enemy);
        }
        Destroy(this.gameObject);
    }

    public void StartSpawning() {
        playerEnteredRoom = true;
        enemiesToSpawn = Random.Range(minEnemiesSpawned, maxEnemiesSpawned);
        timeSinceLastSpawn = timeBetweenSpawns;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        foreach(Transform spawnPoint in transform) {
            if(spawnPoint.tag == "SpawnPoint") {
                Gizmos.DrawWireSphere(spawnPoint.position, 1f);
            }
        }
        Gizmos.color = Color.red;
    }

}
