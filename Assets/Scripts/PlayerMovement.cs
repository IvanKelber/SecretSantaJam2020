using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : RaycastController
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

    public void Move(Vector2 displacement) {

        HorizontalCollisions(ref displacement);
        VerticalCollisions(ref displacement);
        Vector3 velocity = new Vector3(displacement.x, displacement.y, 0);
        transform.position += velocity;
    }

    public void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                moveAmount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
            }
        }
    }

    public void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        if (Mathf.Abs(moveAmount.y) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            if (hit)
            {
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
            }
        }
    }

    void UpdateMousePosition() {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
    }

}
