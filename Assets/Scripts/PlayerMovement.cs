using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Controller2D
{

    PlayerValues playerValues;

    [SerializeField]
    Camera cam;

    public GameObject playerCursor;
    public GameObject cameraFocus;
    [Range(0,1)]
    public float cameraFocusScale = .5f;

    Vector2 playerInput;

    [HideInInspector]
    public Vector3 mousePosition;

    [HideInInspector]
    public bool flipped = false;
    public bool playerDead = false;
    void Start()
    {
        base.Start();
    }

    void Update()
    {

        UpdateRaycastOrigins();
        UpdateMousePosition();
        if(StaticUserControls.paused || playerDead) {
            return;
        }
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        flipped = Mathf.Sign(mousePosition.x - transform.position.x) < 0;

        transform.localScale = new Vector3(Mathf.Sign(mousePosition.x - transform.position.x), transform.localScale.y, transform.localScale.z);
        Vector2 displacement = playerInput.normalized * Mathf.Max(playerValues.playerMovementSpeed, 1) * Time.deltaTime;

        Move(displacement);

    }

    void UpdateMousePosition() {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        playerCursor.transform.position = mousePosition;
        cameraFocus.transform.position = Vector3.Lerp(transform.position, mousePosition, cameraFocusScale);
    }

    public void SetPlayerValues(PlayerValues values) {
        playerValues = values;
    }

    public bool GetInteractKey() {
        
        return !StaticUserControls.paused && Input.GetKeyDown(KeyCode.E);
    }

    public bool GetShootKey() {
        return !StaticUserControls.paused && Input.GetMouseButton(0);
    }

}
