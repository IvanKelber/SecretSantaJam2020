using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGizmo : MonoBehaviour
{
    public float height;
    public float width;


    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(width,height,1));
    }
}
