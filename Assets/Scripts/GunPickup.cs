using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    public GunConfig config;
    [SerializeField]
    LayerMask playerMask;
    [SerializeField]
    AudioManager audioManager;

    AudioSource audioSource;

    Player nearbyPlayer;

    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            nearbyPlayer = collision.gameObject.GetComponent<Player>();
            nearbyPlayer.AddNearbyGun(this);
        }
    }
    
    void OnTriggerExit2D() {
        if(nearbyPlayer != null) { 
            nearbyPlayer.RemoveNearbyGun(this);
            nearbyPlayer = null;
        }
    }

    public void Destroy() {
        audioManager.Play("GunPickup", audioSource);
        Destroy(this.gameObject);
    }

}
