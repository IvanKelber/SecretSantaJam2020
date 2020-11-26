using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Dungeon : MonoBehaviour
{
    public int maxRoomsX = 5;
    public int maxRoomsY = 5;
    public bool[,] grid;

    public float roomWidth = 10;
    public float roomHeight = 10;
    public int roomPadding = 1;

    public int numberOfRooms = 10;

    public Vector2 originRoom;
    public GameObject wallPrefab;
    public List<GameObject> openingPrefabs = new List<GameObject>();

    public List<RoomConfig> roomConfigs;

    public Tilemap tileMap;
    public Tileset tileSet;
    Vector2 startRoom;
    Vector2 endRoom;

    int currentRooms = 0;

    // public void Start() {
    //     GenerateGrid();
    // }

    // public void Update() {
    //     if(Input.GetKeyDown(KeyCode.Space)) {
    //         GenerateGrid();
    //     }
    // }

    public void GenerateGrid() {
        grid = new bool[maxRoomsX, maxRoomsY];
        currentRooms = 0;
        List<Vector2> availableRooms = new List<Vector2>();
        availableRooms.Add(originRoom);
        tileMap.ClearAllTiles();

        while(currentRooms < numberOfRooms) {
            // choose a random room that's available
            int roomIndex = Random.Range(0, availableRooms.Count);
            Vector2 room = availableRooms[roomIndex];
            availableRooms.RemoveAt(roomIndex);
            grid[(int)room.x, (int)room.y] = true;
            currentRooms++;
            AddAdjacentRooms(ref availableRooms, room);
            FillRoom(room);
        }
        foreach(Transform child in transform) {
            if(child.gameObject.tag != "Grid")
                Destroy(child.gameObject);
        }
        PlaceWalls();
        ChooseStartAndEnd();
    }


    void AddAdjacentRooms(ref List<Vector2> availableRooms, Vector2 room) {
        for(int i = -1; i <= 1; i++) {
            for(int j = -1; j <= 1; j++) {
                if((i + j)  % 2 == 0) {
                    continue;
                }
                Vector2 adjacentRoom = new Vector2(room.x + i, room.y + j);
                if(adjacentRoom.x >= maxRoomsX || adjacentRoom.x < 0 || adjacentRoom.y >= maxRoomsY || adjacentRoom.y < 0) {
                    continue;
                }

                if(!availableRooms.Contains(adjacentRoom) && !grid[(int)adjacentRoom.x, (int)adjacentRoom.y]) {
                    availableRooms.Add(adjacentRoom);
                }
            }
        }
    }

    void PlaceWalls() {
        for(int i = 0; i < maxRoomsX; i++) {
            for(int j = 0; j < maxRoomsY; j++) {
                if(grid[i,j]) {
                    Vector3 roomCenter = GetRoomCenter(i,j);
                    Vector3 rightWall = roomCenter + roomWidth/2 * Vector3.right;
                    Vector3 leftWall = roomCenter + roomWidth/2 * Vector3.left;
                    Vector3 topWall = roomCenter + roomHeight/2 * Vector3.up;
                    Vector3 bottomWall = roomCenter + roomHeight/2 * Vector3.down;

                    //Right
                    GameObject rightPrefab = (i + 1 >= maxRoomsX || !grid[i+1,j]) ? wallPrefab : null;
                    PlaceWall(rightWall, rightPrefab, true);

                    //Left
                    GameObject leftPrefab = (i - 1 < 0 || !grid[i-1,j]) ? wallPrefab : null;
                    PlaceWall(leftWall, leftPrefab, true);

                    //Top
                    GameObject topPrefab = (j + 1 >= maxRoomsY || !grid[i,j+1]) ? wallPrefab : null;
                    PlaceWall(topWall, topPrefab, false);

                    //Bottom
                    GameObject bottomPrefab = (j - 1 < 0 || !grid[i,j-1]) ? wallPrefab : null;
                    PlaceWall(bottomWall, bottomPrefab, false);
                }
                
            }
        }

        for(int i = 0; i < maxRoomsX; i++) {
            for(int j = 0; j < maxRoomsY; j++) {
                if(grid[i,j]) {
                    Vector3 roomCenter = GetRoomCenter(i,j);
                    Vector3 rightWall = roomCenter + roomWidth/2 * Vector3.right;
                    Vector3 leftWall = roomCenter + roomWidth/2 * Vector3.left;
                    Vector3 topWall = roomCenter + roomHeight/2 * Vector3.up;
                    Vector3 bottomWall = roomCenter + roomHeight/2 * Vector3.down;

                    //Right
                    GameObject rightPrefab = (i + 1 >= maxRoomsX || !grid[i+1,j]) ? null : RandomOpening();
                    PlaceWall(rightWall, rightPrefab, true);

                    //Bottom
                    GameObject bottomPrefab = (j - 1 < 0 || !grid[i,j-1]) ? null : RandomOpening();
                    PlaceWall(bottomWall, bottomPrefab, false);
                }
                
            }
        }
    }

    public void FillRoom(Vector2 room) {
        for(int i = 0; i < roomWidth; i++) {
            for(int j = 0; j < roomHeight; j++) {
                Vector3Int tileLocation = new Vector3Int(
                    (int)(room.x * roomWidth) - (int)roomWidth/2 + i,
                    (int)(room.y * roomHeight) -(int) roomHeight/2+ j,
                    1);
                tileMap.SetTile(tileLocation, tileSet.Get("Snow"));
            }
        }
        GameObject interiorPrefab = roomConfigs[Random.Range(0, roomConfigs.Count)].interiorPrefab;
        if(interiorPrefab != null) {
            GameObject roomInterior = Instantiate(interiorPrefab, GetRoomCenter(room), Quaternion.identity);
            Debug.Log("Instantiating roomInterior", roomInterior);

            roomInterior.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
            // roomInterior.transform.parent = this.transform;
        }
    }

    GameObject RandomOpening() {
        int index = Random.Range(0, openingPrefabs.Count);
        if(index == openingPrefabs.Count) {
            return null;
        }
        return openingPrefabs[index];
    }

    void PlaceWall(Vector3 wallCenter, GameObject prefab, bool vertical) {
        if(prefab == null) {
            return;
        }
        // wallCenter.z -= 1;
        GameObject wallObj = Instantiate(prefab, wallCenter, Quaternion.Euler(0,0,vertical ? 0 : 90));
        wallObj.transform.localScale = new Vector3(wallObj.transform.localScale.x, (vertical ? roomHeight : roomWidth) + 1, wallObj.transform.localScale.z);
        wallObj.transform.parent = this.transform;
    }

    void ChooseStartAndEnd() {
        bool startRoomChosen = false;
        for(int i = 0; i < maxRoomsX; i++) {
            for(int j = 0; j < maxRoomsY; j++) {
                if(grid[i,j]) {
                    if(!startRoomChosen) {
                        startRoom = new Vector2(i,j);
                        startRoomChosen = true;
                    }
                    endRoom = new Vector2(i,j);
                    break;
                }
            }

        }
    }

    public Vector3 GetRoomCenter(Vector2 room) {
        return GetRoomCenter((int)room.x, (int)room.y);
    }

    public Vector3 GetRoomCenter(int i, int j) {
        return transform.position + ((i * roomWidth + roomPadding)* Vector3.right) + 
                                    ((j * roomHeight + roomPadding) * Vector3.up );
    }

    public Vector3 GetStartRoom() {
        return GetRoomCenter(startRoom);
    }

    public Vector3 GetEndRoom() {
        return GetRoomCenter(endRoom);
    }

    void OnDrawGizmos() {
        if(grid == null) {
            return;
        }
        for(int i = 0; i < maxRoomsX; i++) {
            for(int j = 0; j < maxRoomsY; j++) {
                Vector3 roomCenter = GetRoomCenter(i,j);
                Gizmos.color = grid[i,j] ? new Color(1,0,0,.1f) : new Color(0,0,0,.1f);
                if(new Vector2(i,j) == startRoom) {
                    Gizmos.color = new Color(0,1,0,.1f);
                }
                if(new Vector2(i,j) == endRoom) {
                    Gizmos.color = new Color(0,0,1,.1f);
                }
                Gizmos.DrawCube(roomCenter,new Vector3(roomWidth, roomHeight, 1));
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(roomCenter, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }

}
