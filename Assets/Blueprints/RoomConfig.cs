using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[CreateAssetMenu(menuName="Configs/Room")]
public class RoomConfig : ScriptableObject
{
    public string name;
    public bool isSpecific = false;
    public bool infinteEnemySpawn = false;

    public bool spawnEnemies = false;

    [Range(0,20)]
    public int minNumberOfEnemies = 1;
    [Range(0,20)]
    public int maxNumberOfEnemies = 1;

    [Range(0,20)]
    public int maxEnemiesInRoomAtOnce = 1;

    [Range(1,20)]
    public float spawnRate = 5;

    public GameObject interiorPrefab;

    public List<GameObject> enemiesToSpawn;

    public int numberOfWaves = 2;

    public BoolGameEvent lockDoors;

}
