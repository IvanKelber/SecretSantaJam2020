using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Configs/Room")]
public class RoomConfig : ScriptableObject
{
    public string name;
    public bool isSpecific = false;
    public bool infinteEnemySpawn = false;

    [Range(0,20)]
    public int numberOfEnemies = 1;

    [Range(0,5)]
    public int numberOfPickups = 1;

    public GameObject interiorPrefab;



}
