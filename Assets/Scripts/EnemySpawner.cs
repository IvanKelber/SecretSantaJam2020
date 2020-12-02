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
    private int totalWaves;
    private int minEnemiesSpawned;
    private int maxEnemiesSpawned;
    private float timeBetweenSpawns;
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    private int wavesSpawned = 0;

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
        if(timeSinceLastSpawn >= timeBetweenSpawns && wavesSpawned < totalWaves) {
            timeSinceLastSpawn = 0;
            StartCoroutine(PrepareSpawn());
            wavesSpawned++;
            Debug.Log("Spawning wave: " + wavesSpawned);
        } else if(wavesSpawned == totalWaves) {
            //The room has been completed
        }
        timeSinceLastSpawn += Time.deltaTime;
    }

    public void SetRoomConfig(RoomConfig config) {
        minEnemiesSpawned = config.minNumberOfEnemies;
        maxEnemiesSpawned = config.maxNumberOfEnemies;
        timeBetweenSpawns = config.spawnRate;
        enemyPrefabs = config.enemiesToSpawn;
        totalWaves = config.numberOfWaves;
        if(enemyPrefabs.Count == 0 || maxEnemiesSpawned == 0) {
            Destroy(this);
        }
    }

    public IEnumerator PrepareSpawn() {
        enemiesToSpawn = Random.Range(minEnemiesSpawned, maxEnemiesSpawned);
        List<Transform> spawns = SelectSpawnPointsForWave(enemiesToSpawn, spawnPoints);
        foreach(Transform spawnPoint in spawns) {
            int index = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[index];
            StartCoroutine(Spawn(enemyPrefab, spawnPoint));
            yield return new WaitForSeconds(Random.Range(0,.1f));
        }       
        yield return null;
    }

    public IEnumerator Spawn(GameObject enemyPrefab, Transform spawnPoint) {
        GameObject spawnIndicator = Instantiate(enemyPrefab.GetComponent<AIEntity>().AIconfig.spawnIndicator, spawnPoint.position, Quaternion.identity);
        spawnIndicator.transform.parent = spawnPoint;
        yield return new WaitForSeconds(spawnDuration);
        Destroy(spawnIndicator);
        yield return null;        
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemy.transform.parent = spawnPoint;
        enemy.transform.localScale = Vector3.one;
        totalEnemiesSpawned++;
        yield return null;
    }


    public void ClearEnemies() {
        foreach(GameObject enemy in spawnedEnemies) {
            Destroy(enemy);
        }
        Destroy(this.gameObject);
    }

    public void StartSpawning() {
        if(playerEnteredRoom) {
            return;
        }
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

    List<Transform> SelectSpawnPointsForWave(int sizeOfWave, List<Transform> spawnPoints) {
        if(sizeOfWave >= spawnPoints.Count) {
            return spawnPoints;
        }
        // for (int i = 0; i < spawnPoints.Count; i++) {
        //     Transform temp = spawnPoints[i];
        //     int randomIndex = Random.Range(i, spawnPoints.Count);
        //     spawnPoints[i] = spawnPoints[randomIndex];
        //     spawnPoints[randomIndex] = temp;
        // }
        List<Transform> chosenPoints = new List<Transform>();
        // for(int i = 0; i < sizeOfWave; i++) {
        //     chosenPoints.Add(spawnPoints[i]);
        // }
        // return chosenPoints;

        int pointsLeftToPick = sizeOfWave;
        int pointCount = spawnPoints.Count;
        Debug.Log("Selecting spawn points for wave");

        foreach(Transform spawnPoint in spawnPoints) {
            Debug.Log("probability: " + pointsLeftToPick + "/" + pointCount);
            if(Random.Range(0,pointCount - 1) < pointsLeftToPick) {
                // Choose the element with probablility pointsLeftToPick/pointCount
                chosenPoints.Add(spawnPoint);
                pointsLeftToPick--;
            }
            pointCount--;
            if(pointsLeftToPick == 0) {
                break;
            }
            if(pointCount == 0) {
                Debug.LogError("Failed to choose " + sizeOfWave + " spawn points");
                break;
            }
        }
        return chosenPoints;
    }

}
