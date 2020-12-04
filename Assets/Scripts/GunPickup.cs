using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : Interactable
{

    public GunConfig config;

    [SerializeField]
    protected AudioManager audioManager;

    protected AudioSource audioSource;

    Player nearbyPlayer;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    [Range(.5f, 5)]
    float promptDistance = 1;

    public void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        spriteRenderer.sprite = config.gunSprite;
    }
  
    public void Update() {
        base.Update();
        UpdateInteractPrompt();
    }

    public override void OnEnterTrigger(Collider2D collision) {
        nearbyPlayer = collision.gameObject.GetComponent<Player>();
    }


    public override void OnExitTrigger(Collider2D collider)
    {
        if(nearbyPlayer != null) {
            nearbyPlayer = null;
        }        
    }

    public override void OnInteract()
    {
        if(StaticUserControls.paused) {
            return;
        }
        base.OnInteract();
        StartCoroutine(Destroy());
    }

    public IEnumerator Destroy() {
        spriteRenderer.color = Color.clear;
        yield return StartCoroutine(audioManager.PlayAndWait("GunPickup", audioSource));
        Destroy(this.gameObject);
    }

    public void UpdateInteractPrompt() {
        if(nearbyPlayer != null && interactPrompt != null) {
            Vector3 promptPosition = transform.position + (transform.position - nearbyPlayer.transform.position).normalized * promptDistance;
            interactPrompt.transform.position = promptPosition;
        }
    }

}
