﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Interactable : MonoBehaviour, IInteractable
{

    public LayerMask playerMask;
    public GameObject interactPrompt;
    protected bool playerPresent = false;

    public virtual void Start() {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public virtual void Update() {
        if(playerPresent && Input.GetKeyDown(KeyCode.E)) {
            OnInteract();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            playerPresent = true;
            if(interactPrompt != null)
                interactPrompt.SetActive(true);
            OnEnterTrigger(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            playerPresent = false;
            if(interactPrompt != null)
                interactPrompt.SetActive(false);
            OnExitTrigger(collision);
        }
    }

    public virtual void OnInteract() {
        //nothing
        Destroy(interactPrompt);
    }

    public virtual void OnEnterTrigger(Collider2D collider) {}
    public virtual void OnStayTrigger(Collider2D collider) {}
    public virtual void OnExitTrigger(Collider2D collider) {}
}
