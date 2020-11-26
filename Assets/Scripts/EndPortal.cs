using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EndPortal : MonoBehaviour
{
    public GameEvent levelComplete;
    public LayerMask playerMask;
    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Trigger entered");
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            levelComplete.Raise();
        }
    }
}
