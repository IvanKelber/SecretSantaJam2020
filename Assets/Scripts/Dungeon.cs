using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dungeon : MonoBehaviour
{
    public int maxRoomsX = 5;
    public int maxRoomsY = 5;
    public Room[,] grid;

    public float roomWidth = 10;
    public float roomHeight = 10;
    public int wallWidth = 1;

    public int numberOfRooms = 10;

    public Vector2 originRoom;
    public GameObject verticalWallPrefab;
    public GameObject horizontalWallPrefab;
    public List<GameObject> horizontalOpeningPrefabs = new List<GameObject>();
    public List<GameObject> verticalOpeningPrefabs = new List<GameObject>();

    public RoomManifest roomManifest;
    public Tilemap tileMap;
    public Tileset tileSet;
    Vector2 startRoom;
    Vector2 endRoom;

    int currentRooms = 0;

    [SerializeField]
    private bool testDungeon = false;

    public void Start() {
        if(testDungeon)
            GenerateLevel(0);
    }

    public void Update() {
        if(testDungeon && Input.GetKeyDown(KeyCode.Space)) {
            GenerateLevel(0);
        }
    }

    public void GenerateLevel(int level) {
        //TODO update levelManifest (rooms, guns, enemies, tiles, etc.)
        GenerateGrid();
    }

    public void DestroyCurrentLevel() {
        grid = new Room[maxRoomsX, maxRoomsY];
        currentRooms = 0;
        tileMap.ClearAllTiles();
        foreach(Transform child in transform) {
            if(child.gameObject.tag != "Grid")
                Destroy(child.gameObject);
        }
    }

    public void GenerateGrid() {
        DestroyCurrentLevel();
        List<Vector2> availableRooms = new List<Vector2>();
        availableRooms.Add(originRoom);

        while(currentRooms < numberOfRooms) {
            // choose a random room that's available
            int roomIndex = Random.Range(0, availableRooms.Count);
            Vector2 room = availableRooms[roomIndex];
            availableRooms.RemoveAt(roomIndex);
            grid[(int)room.x, (int)room.y] = new Room(roomManifest.GetRandomNonSpecific());
            currentRooms++;
            AddAdjacentRooms(ref availableRooms, room);
        }
        FillRooms();
        FillAbyss();

        PlaceWalls();
    }
     
    void FillRooms() {
        int roomCount = 0;
        for(int i = 0; i < maxRoomsX; i++) {
            for(int j = 0; j < maxRoomsY; j++) {
                if(grid[i,j]) {
                    if(roomCount == 0) {
                        grid[i,j].SetConfig(roomManifest.Get("StartRoom"));
                        startRoom = new Vector2(i,j);
                    } else if(roomCount == numberOfRooms - 1) {
                        grid[i,j].SetConfig(roomManifest.Get("EndRoom"));
                        endRoom = new Vector2(i,j);
                    }
                    grid[i,j].Instantiate(this.transform, GetRoomCenter(i,j));
                    roomCount++;
                }
            }
        }
    }

    void FillAbyss() {
        for(int x = 0; x < maxRoomsX; x++) {
            for(int y = 0; y < maxRoomsY; y++) {
                if(!grid[x,y]) {
                    
                    for(int i = 0; i < roomWidth + wallWidth; i++) {
                        for(int j = 0; j < roomHeight + wallWidth; j++) {
                            Vector3Int tileLocation = new Vector3Int(
                                (int)(x * (roomWidth + wallWidth) + i - roomWidth/2),
                                (int)(y * (roomHeight + wallWidth) + j - roomHeight/2),
                                1);
                            tileMap.SetTile(tileLocation, tileSet.Get("Snow"));
                        }
                    } 
                }
            }
        }
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
        // Spawn exterior walls (i.e. level boundaries)
        for(int i = 0; i < maxRoomsX; i++) {
            for(int j = 0; j < maxRoomsY; j++) {
                if(grid[i,j]) {
                    Vector3 roomCenter = GetRoomCenter(i,j);
                    Vector3 rightWall = roomCenter + (roomWidth + wallWidth)/2 * Vector3.right;
                    Vector3 leftWall = roomCenter + (roomWidth + wallWidth)/2 * Vector3.left;
                    Vector3 topWall = roomCenter + (roomHeight + wallWidth)/2 * Vector3.up;
                    Vector3 bottomWall = roomCenter + (roomHeight + wallWidth)/2 * Vector3.down;

                    //Right
                    GameObject rightPrefab = (i + 1 >= maxRoomsX || !grid[i+1,j]) ? verticalWallPrefab : null;
                    PlaceWall(rightWall, rightPrefab);

                    //Left
                    GameObject leftPrefab = (i - 1 < 0 || !grid[i-1,j]) ? verticalWallPrefab : null;
                    PlaceWall(leftWall, leftPrefab);

                    //Top
                    GameObject topPrefab = (j + 1 >= maxRoomsY || !grid[i,j+1]) ? horizontalWallPrefab : null;
                    PlaceWall(topWall, topPrefab);

                    //Bottom
                    GameObject bottomPrefab = (j - 1 < 0 || !grid[i,j-1]) ? horizontalWallPrefab : null;
                    PlaceWall(bottomWall, bottomPrefab);
                }
                
            }
        }

        // Spawn interior wall openings
        for(int i = 0; i < maxRoomsX; i++) {
            for(int j = 0; j < maxRoomsY; j++) {
                if(grid[i,j]) {
                    Vector3 roomCenter = GetRoomCenter(i,j);
                    Vector3 rightWall = roomCenter + (roomWidth + wallWidth)/2 * Vector3.right;
                    Vector3 leftWall = roomCenter + (roomWidth + wallWidth)/2 * Vector3.left;
                    Vector3 topWall = roomCenter + (roomHeight + wallWidth)/2 * Vector3.up;
                    Vector3 bottomWall = roomCenter + (roomHeight + wallWidth)/2 * Vector3.down;

                    //Right
                    GameObject rightPrefab = (i + 1 >= maxRoomsX || !grid[i+1,j]) ? null : RandomOpening(false);
                    PlaceWall(rightWall, rightPrefab);

                    //Bottom
                    GameObject bottomPrefab = (j - 1 < 0 || !grid[i,j-1]) ? null : RandomOpening(true);
                    PlaceWall(bottomWall, bottomPrefab);
                }
                
            }
        }
    }

    GameObject RandomOpening(bool horizontal) {
        if(horizontal) {
            int index = Random.Range(0, horizontalOpeningPrefabs.Count);

            return horizontalOpeningPrefabs[index]; 
        } else {
            int index = Random.Range(0, verticalOpeningPrefabs.Count);

            return verticalOpeningPrefabs[index];  
        }
    }

    void PlaceWall(Vector3 wallCenter, GameObject prefab) {
        if(prefab == null) {
            return;
        }
        GameObject wallObj = Instantiate(prefab, wallCenter, Quaternion.identity);
        wallObj.transform.parent = this.transform;
    }


    public Vector3 GetRoomCenter(Vector2 room) {
        return GetRoomCenter((int)room.x, (int)room.y);
    }

    public Vector3 GetRoomCenter(int i, int j) {
        return transform.position + (i * (roomWidth + wallWidth) * Vector3.right) + (j * (roomHeight + wallWidth) * Vector3.up);
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
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(roomCenter, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }

}
