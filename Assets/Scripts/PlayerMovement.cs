using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : RaycastController
{

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    Camera cam;

    [SerializeField]
    float playerSpeed = 10;

    [SerializeField]
    float shotRate = .5f;
    
    [SerializeField]
    GameObject bulletPrefab;

    AudioSource audioSource;
    float timeSinceLastShot = 0;

    Vector2 playerInput;

    void Start() {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateRaycastOrigins();
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.GetMouseButton(0) && timeSinceLastShot > shotRate) {
            Shoot();
        }
        timeSinceLastShot += Time.deltaTime;
        Move();
    }

    void Move() {

        Vector2 displacement = playerInput.normalized * playerSpeed;
    
        HorizontalCollisions(ref displacement);
        VerticalCollisions(ref displacement);
        Vector3 velocity = new Vector3(displacement.x, displacement.y, 0) * Time.deltaTime;
        transform.position += velocity;
    }

     void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = 2 * skinWidth;

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

    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = 2 * skinWidth;

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

    void Shoot() {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>() as Bullet;
        audioManager.Play("BulletBirth", audioSource);

        bullet.SetDirection((mousePosition - transform.position).normalized);
        timeSinceLastShot = 0;
    }
}
