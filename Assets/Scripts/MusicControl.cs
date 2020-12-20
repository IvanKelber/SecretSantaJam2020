using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicControl : MonoBehaviour
{
    
    [SerializeField]
    AudioManager musicManager;

    AudioSource audioSource;

    public float fadeDuration;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; //Always loop music from this audio source.
        musicManager.Play("MainTheme", audioSource);
    }

    public void PlayMain() {
        StartCoroutine(FadeMusic("MainTheme"));
    }

    public void PlayBoss() {
        StartCoroutine(FadeMusic("BossTheme"));
    }

    IEnumerator FadeMusic(string theme) {
        Tween fade = audioSource.DOFade(0, fadeDuration);
        yield return fade.WaitForCompletion();
        audioSource.Stop();
        audioSource.volume = 1;
        musicManager.Play(theme, audioSource);
    }
}
