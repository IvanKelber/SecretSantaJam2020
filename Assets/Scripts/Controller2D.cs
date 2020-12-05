using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController
{

    private float knockbackDuration = .05f;

    void Start()
    {
        base.Start();

    }

    public void Move(Vector2 displacement) {
        if(StaticUserControls.paused) {
            return;
        }
        HorizontalCollisions(ref displacement);
        VerticalCollisions(ref displacement);
        Vector3 velocity = new Vector3(displacement.x, displacement.y, 0);
        transform.position += velocity;
    }

    public void Force(Vector3 force) {
        StartCoroutine(SustainedMove(force));
    }

    public IEnumerator SustainedMove(Vector3 force) {
        float magnitude = force.magnitude;
        Vector3 direction = force.normalized;

        float startTime = Time.time;
        float endTime = startTime + knockbackDuration;
        while(Time.time < endTime) {
            float percentage = Ease((endTime - Time.time)/knockbackDuration);
            float instantMagnitude = Mathf.Lerp(0, magnitude,percentage);
            Vector3 displacement = direction * instantMagnitude * Time.deltaTime;
            Move((Vector2) displacement);
            yield return null;
        }
    }

    public float Ease(float x) {
        return 1 - Mathf.Pow(2, -10 * x);
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

}
