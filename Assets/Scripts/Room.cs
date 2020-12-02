using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{


    private RoomConfig config;
    private GameObject room;
    
    private Vector2Int roomIndex;

    private List<GameObject> openings = new List<GameObject>();

    public Room(RoomConfig config, int i, int j) {
        this.config = config;
        roomIndex = new Vector2Int(i,j);
    }

    public Room(RoomConfig config, Vector2 room) : this(config, (int) room.x, (int) room.y){
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
        RoomBehavior rb = room.GetComponent<RoomBehavior>();
        rb.SetConfig(config);
    }

    public static implicit operator bool(Room room) {
        return room != null && room.config != null;
    }
    
}
