using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInteriorGizmo : MonoBehaviour
{
    public float roomWidth = 30;
    public float roomHeight = 18;

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(roomWidth, roomHeight,1));
    }
}
