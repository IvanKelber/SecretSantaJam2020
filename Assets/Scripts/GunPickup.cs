using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    public GunConfig config;
    [SerializeField]
    LayerMask playerMask;

    PlayerMovement nearbyPlayer;

    void OnTriggerEnter2D(Collider2D collision) {
        if((playerMask.value & 1 << collision.gameObject.layer) != 0) {
            nearbyPlayer = collision.gameObject.GetComponent<PlayerMovement>();
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
        Destroy(this.gameObject);
    }

}
