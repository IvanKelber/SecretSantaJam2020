using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Manifest/Rooms")]
public class RoomManifest : Manifest<RoomConfig>
{
    public RoomConfig GetRandomNonSpecific() {
        List<RoomConfig> nonSpecificRooms = new List<RoomConfig>();
        foreach(RoomConfig room in items) {
            if(!room.isSpecific) {
                nonSpecificRooms.Add(room);
            }
        }
        return nonSpecificRooms[Random.Range(0, nonSpecificRooms.Count)];
    }
}
