using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EnemySpawner : MonoBehaviour
{

    public LayerMask playerMask;
    public float spawnDuration = 1;
    public GameEvent roomCompleteEvent;

    List<GameObject> spawnedEnemies = new List<GameObject>();

    List<Transform> spawnPoints = new List<Transform>();

    private int totalEnemiesSpawned = 0;
    private float timeSinceLastSpawn = 0;
    private bool playerEnteredRoom = false;
    private int enemiesToSpawn;
    private bool doneSpawning = false;
    private int aliveEnemies;
    private bool roomComplete = false;
    private bool spawning = false;

    //Config
    private int totalWaves;
    private int minEnemiesSpawned;
    private int maxEnemiesSpawned;
    private float timeBetweenSpawns;
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    private int wavesSpawned = 0;
    private int difficulty = 0;

    void Start() {
        timeSinceLastSpawn = timeBetweenSpawns;
        foreach(Transform child in transform) {
            if(child.gameObject.activeSelf && child.gameObject.tag == "SpawnPoint") {
                spawnPoints.Add(child);
            }
        }
    }

    public void SetDifficulty(int difficulty) {
        this.difficulty = difficulty;
    }

    void Update()
    {
        if(StaticUserControls.paused || !playerEnteredRoom || roomComplete) {
            return;
        }
        CountAliveEnemies();

        if(doneSpawning) {
            if(aliveEnemies == 0 && !spawning) {
                roomComplete = true;
                roomCompleteEvent.Raise();
                Destroy(this.gameObject);
            }
        } else if(timeSinceLastSpawn >= timeBetweenSpawns || (!spawning && aliveEnemies == 0) ) {
            timeSinceLastSpawn = 0;
            spawning = true;
            StartCoroutine(PrepareSpawn());
            wavesSpawned++;
        } else if(wavesSpawned == totalWaves) {
            //The room has been completed
            doneSpawning = true;
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
            Debug.LogWarning("EnemySpawner is present but shouldn't be");
            Destroy(this.gameObject);
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
        enemy.GetComponent<AIEntity>().SetDifficulty(difficulty);
        spawnedEnemies.Add(enemy);
        totalEnemiesSpawned++;
        yield return null;
        spawning = false;
    }

    void CountAliveEnemies() {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
        aliveEnemies = spawnedEnemies.Count;
    }

    public void StartSpawning() {
        // if(playerEnteredRoom) {
        //     return;
        // }
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

        for (int i = 0; i < spawnPoints.Count; i++) {
            Transform temp = spawnPoints[i];
            int randomIndex = Random.Range(i, spawnPoints.Count);
            spawnPoints[i] = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = temp;
        }
        List<Transform> chosenPoints = new List<Transform>();
        for(int i = 0; i < sizeOfWave; i++) {
            chosenPoints.Add(spawnPoints[i]);
        }
        return chosenPoints;

    }

}
