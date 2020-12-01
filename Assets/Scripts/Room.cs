using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private RoomConfig config;
    private GameObject room;
    
    public Room(RoomConfig config) {
        this.config = config;
    }

    public void SetConfig(RoomConfig config) {
        this.config = config;
    }

    public void Instantiate(Transform parent, Vector3 center) {
        if(config == null) {
            return;
        }
        room = GameObject.Instantiate(config.interiorPrefab, center, Quaternion.identity);
        room.transform.parent = parent;
        room.GetComponent<RoomBehavior>().SetConfig(config);
    }

    public static implicit operator bool(Room room) {
        return room != null && room.config != null;
    }
    
}
