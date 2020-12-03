using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Interactable
{

    [SerializeField]
    int minValue = 5;
    [SerializeField]
    int maxValue = 10;

    [SerializeField]
    PlayerValues playerValues;

    [SerializeField]
    AudioManager audioManager;

    AudioSource audioSource;

    int value;

    public void Awake() {
        value = Random.Range(minValue, maxValue);
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public override void OnEnterTrigger(Collider2D collider) {
        playerValues.goldCount += value;
        StartCoroutine(Destroy());
    }

    public IEnumerator Destroy() {
        GetComponent<SpriteRenderer>().color = Color.clear;
        yield return StartCoroutine(audioManager.PlayAndWait("GoldCollect", audioSource));
        Destroy(gameObject);
    }
}
