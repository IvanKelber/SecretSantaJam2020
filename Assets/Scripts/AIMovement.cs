using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : Controller2D
{
    AIEntity entity;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        entity = GetComponent<AIEntity>();
    }

    void Update() {
        UpdateRaycastOrigins();
    }

    
}
