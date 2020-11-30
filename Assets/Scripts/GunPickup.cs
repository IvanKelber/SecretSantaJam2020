using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : Interactable
{

    public GunConfig config;

    [SerializeField]
    AudioManager audioManager;

    AudioSource audioSource;

    Player nearbyPlayer;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    [Range(.5f, 5)]
    float promptDistance = 1;

    [ExecuteInEditMode]
    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        spriteRenderer.sprite = config.gunSprite;
    }
  
    void Update() {
        base.Update();
        UpdateInteractPrompt();
    }

    public override void OnEnterTrigger(Collider2D collision) {
        nearbyPlayer = collision.gameObject.GetComponent<Player>();
        nearbyPlayer.AddNearbyGun(this);
    }


    public override void OnExitTrigger(Collider2D collider)
    {
        if(nearbyPlayer != null) {
            nearbyPlayer.RemoveNearbyGun(this);
            nearbyPlayer = null;
        }        
    }

    public override void OnInteract()
    {
        base.OnInteract();
        nearbyPlayer.PickupGun();
    }

    public void Destroy() {
        audioManager.Play("GunPickup", audioSource);
        Destroy(this.gameObject);
    }

    public void UpdateInteractPrompt() {
        if(nearbyPlayer != null) {
            Vector3 promptPosition = transform.position + (transform.position - nearbyPlayer.transform.position).normalized * promptDistance;
            interactPrompt.transform.position = promptPosition;
        }
    }

}
