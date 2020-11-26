using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInteriorGizmo : MonoBehaviour
{
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(1,1,1));
    }
}
