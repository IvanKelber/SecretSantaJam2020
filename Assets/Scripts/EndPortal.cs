using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EndPortal : MonoBehaviour
{
    public GameEvent levelComplete;
    public LayerMask playerMask;

    bool playerPresent = false;

    void Update() {
        if(playerPresent && Input.GetKeyDown(KeyCode.E)) {
            levelComplete.Raise();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            playerPresent = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            playerPresent = false;
        }
    }
}
