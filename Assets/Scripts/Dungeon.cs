using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public int maxRoomsX = 5;
    public int maxRoomsY = 5;
    public bool[,] grid;

    public float roomWidth = 10;
    public float roomHeight = 10;

    public int numberOfRooms = 10;

    public Vector2 originRoom;
    public GameObject wallPrefab;
    public List<GameObject> openingPrefabs = new List<GameObject>();

    Vector2 startRoom;
    Vector2 endRoom;


    List<GameObject> walls = new List<GameObject>();

    int currentRooms = 0;

    public void Start() {
    }

    public void GenerateGrid() {
        grid = new bool[maxRoomsX, maxRoomsY];
        currentRooms = 0;
        List<Vector2> availableRooms = new List<Vector2>();
        availableRooms.Add(originRoom);

        while(currentRooms < numberOfRooms) {
            // choose a random room that's available
            int roomIndex = Random.Range(0, availableRooms.Count);
            Vector2 room = availableRooms[roomIndex];
            availableRooms.RemoveAt(roomIndex);
            grid[(int)room.x, (int)room.y] = true;
            currentRooms++;
            AddAdjacentRooms(ref availableRooms, room);
        }
        foreach(GameObject go in walls) {
            Destroy(go);
        }
        walls = new List<GameObject>();
        openingPrefabs.Add(new GameObject());
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
                    GameObject rightPrefab = (i + 1 >= maxRoomsX || !grid[i+1,j]) ? wallPrefab : RandomOpening();
                    PlaceWall(rightWall, rightPrefab, true);

                    GameObject leftPrefab = (i - 1 < 0 || !grid[i-1,j]) ? wallPrefab : RandomOpening();
                    PlaceWall(leftWall, leftPrefab, true);

                   
                    GameObject topPrefab = (j + 1 >= maxRoomsY || !grid[i,j+1]) ? wallPrefab : RandomOpening();
                    PlaceWall(topWall, topPrefab, false);

                
                    GameObject bottomPrefab = (j - 1 < 0 || !grid[i,j-1]) ? wallPrefab : RandomOpening();
                    PlaceWall(bottomWall, bottomPrefab, false);

 
                }
                
            }
        }
    }

    GameObject RandomOpening() {
        if(openingPrefabs.Count == 0) {
            return new GameObject();
        }
        return openingPrefabs[Random.Range(0, openingPrefabs.Count - 1)];
    }

    void PlaceWall(Vector3 wallCenter, GameObject prefab, bool vertical) {
        GameObject wallObj = Instantiate(prefab, wallCenter, Quaternion.Euler(0,0,vertical ? 0 : 90));
        wallObj.transform.localScale = new Vector3(wallObj.transform.localScale.x, (vertical ? roomHeight : roomWidth) + 1, wallObj.transform.localScale.z);
        walls.Add(wallObj);
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
        return transform.position + (i * roomWidth * Vector3.right) + (j * roomHeight * Vector3.up);
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
                Gizmos.color = grid[i,j] ? new Color(1,0,0,.5f) : new Color(0,0,0,.1f);
                if(new Vector2(i,j) == startRoom) {
                    Gizmos.color = Color.green;
                }
                if(new Vector2(i,j) == endRoom) {
                    Gizmos.color = Color.blue;
                }
                Gizmos.DrawCube(roomCenter,new Vector3(roomWidth, roomHeight, 1));
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(roomCenter, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }

}
