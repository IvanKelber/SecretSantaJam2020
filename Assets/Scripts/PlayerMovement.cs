using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : RaycastController
{

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    List<GunConfig> gunConfigs = new List<GunConfig>();
    [SerializeField]
    Gun gun;

    [SerializeField]
    Camera cam;

    [SerializeField]
    float playerSpeed = 10;
    
    [SerializeField]
    int gunCapacity = 2;
    
    [SerializeField]
    GameObject bulletPrefab;

    AudioSource audioSource;
    float timeSinceLastShot = 0;

    Vector2 playerInput;

    Vector3 mousePosition;

    int gunIndex = 0;

    List<GunPickup> nearbyGuns = new List<GunPickup>();

    void Start() {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateRaycastOrigins();
        UpdateMousePosition();
        PickupGun();
        EquipGun();
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.localScale = new Vector3(Mathf.Sign(mousePosition.x - playerInput.x), transform.localScale.y, transform.localScale.z);
        if(Input.GetMouseButton(0) && timeSinceLastShot > gun.config.fireRate) {
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

    void UpdateMousePosition() {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
    }

    void EquipGun() {
        for(int i = 1; i <= gunConfigs.Count; i++) {
            if(Input.GetKeyDown("" + i)) {
                gunIndex = i - 1;
                gun.config = gunConfigs[gunIndex];
                return;
            }
        }
    }

    void PickupGun() {
        if(Input.GetKeyDown(KeyCode.E)) {
            if(nearbyGuns.Count > 0 && gunConfigs.Count < gunCapacity) {
                GunPickup pickup = nearbyGuns[nearbyGuns.Count - 1 ];
                gunConfigs.Add(pickup.config);
                gunIndex = gunConfigs.Count - 1;
                gun.config = gunConfigs[gunIndex];
                RemoveNearbyGun(pickup);
                pickup.Destroy();
            }
        }
    }

    public void AddNearbyGun(GunPickup pickup) {
        nearbyGuns.Add(pickup);

    }

    public void RemoveNearbyGun(GunPickup pickup) {
        nearbyGuns.Remove(pickup);
    }

    void Shoot() {
       
        gun.Shoot(mousePosition);
        Vector3 knockbackDirection = (transform.position - mousePosition).normalized;
        Vector2 knockback = gun.config.knockback * new Vector2(knockbackDirection.x, knockbackDirection.y) * Time.deltaTime;
        HorizontalCollisions(ref knockback);
        VerticalCollisions(ref knockback);
        transform.position += new Vector3(knockback.x, knockback.y, 0);
        timeSinceLastShot = 0;
    }
}
