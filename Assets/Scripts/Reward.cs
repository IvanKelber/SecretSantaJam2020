using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Reward : Interactable
{

    [SerializeField]
    PlayerValues playerValues;
    [SerializeField]
    GameObjectGameEvent rewardChosen;

    [SerializeField]
    AudioManager audioManager;
    AudioSource audioSource;

    public RewardConfig config;

    public int cost = 0;
    void Start()
    {
        base.Start();
        cost = 0; //TODO derive this from the pickup
        audioSource = gameObject.AddComponent<AudioSource>();

    }

    public override void OnInteract() {
        if(playerValues.goldCount >= cost) {
            playerValues.goldCount -= cost;
            rewardChosen.Raise(this.gameObject);
            audioManager.Play("CardPickup", audioSource);
            base.OnInteract();
        } else {
            // audioManager.Play("NotEnoughGold", audioSource);
        }
    }

    public virtual void Init(RewardConfig config) {
        this.config = config;
    }

    public void ApplyEffect() {
        config.ApplyEffect(playerValues);
    }

    void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position, 1);
    }
}
