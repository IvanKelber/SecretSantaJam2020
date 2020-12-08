using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarGraphScan : MonoBehaviour
{

    public Transform playerTransform;

    public void UpdateBounds() {
        Bounds updateBounds = new Bounds(playerTransform.position, new Vector3(60,36,0));
        GraphUpdateObject guo = new GraphUpdateObject(updateBounds);
        GridGraph gg = AstarPath.active.data.gridGraph;
        gg.center = playerTransform.position;
        AstarPath.active.Scan();
    }
}
