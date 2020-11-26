using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="RoomManifest")]
public class RoomManifest : ScriptableObject
{
    public List<RoomConfig> rooms;
    private Dictionary<string, RoomConfig> roomMap = new Dictionary<string, RoomConfig>();

    void OnEnable() {
        foreach(RoomConfig room in rooms) {
            roomMap.Add(room.name, room);
        }
    }

    void Refresh() {
        foreach(RoomConfig room in rooms) {
            if(!roomMap.ContainsKey(room.name)) {
                roomMap.Add(room.name, room);
            }
        }
    }

    public RoomConfig Get(string name) {
        return roomMap[name];
    }

    public RoomConfig GetRandomNonSpecific() {
        List<RoomConfig> nonSpecificRooms = new List<RoomConfig>();
        foreach(RoomConfig room in rooms) {
            if(!room.isSpecific) {
                nonSpecificRooms.Add(room);
            }
        }
        return nonSpecificRooms[Random.Range(0, nonSpecificRooms.Count -1)];
    }
}
