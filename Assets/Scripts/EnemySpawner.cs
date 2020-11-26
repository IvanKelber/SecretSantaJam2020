using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Range(0,20)]
    public int minEnemiesSpawned = 1;

    [Range(0,20)]
    public int maxEnemiesSpawned = 12;

    [SerializeField]
    float timeBetweenSpawns = 5;

    [SerializeField]
    List<GameObject> enemyPrefabs = new List<GameObject>();

    List<GameObject> spawnedEnemies = new List<GameObject>();

    List<Transform> spawnPoints = new List<Transform>();

    int enemiesToSpawn;
    int totalEnemiesSpawned = 0;
    float timeSinceLastSpawn = 0;
    void Start() {
        enemiesToSpawn = Random.Range(minEnemiesSpawned, maxEnemiesSpawned);
        timeSinceLastSpawn = timeBetweenSpawns;
        foreach(Transform child in transform) {
            if(child.gameObject.tag == "SpawnPoint") {
                spawnPoints.Add(child);
            }
        }
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn >= timeBetweenSpawns && totalEnemiesSpawned < enemiesToSpawn) {
            timeSinceLastSpawn = 0;
            Spawn();
        }
    }

    public void Spawn() {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
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

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        foreach(Transform spawnPoint in transform) {
            if(spawnPoint.tag == "SpawnPoint") {
                Gizmos.DrawWireSphere(spawnPoint.position, 1f);
            }
        }
    }

}
