using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Controller2D
{

    [SerializeField]
    float playerSpeed = 10;

    [SerializeField]
    Camera cam;

    Vector2 playerInput;

    [HideInInspector]
    public Vector3 mousePosition;
    void Start()
    {
        base.Start();

    }

    void Update()
    {
        UpdateRaycastOrigins();
        UpdateMousePosition();
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.localScale = new Vector3(Mathf.Sign(mousePosition.x - transform.position.x), transform.localScale.y, transform.localScale.z);
        Vector2 displacement = playerInput.normalized * playerSpeed * Time.deltaTime;

        Move(displacement);

    }

    void UpdateMousePosition() {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
    }

}
