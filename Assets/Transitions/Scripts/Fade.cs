using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Fade : MonoBehaviour
{
    [Range(0.1f, 5)]
    public float fadeDuration;

    public GameEvent fadeOutComplete, fadeInComplete;
    private ScreenTransitionImageEffect imageEffect;

    bool waitingForUserInput = false;
    bool fading = false;
    void Start()
    {
        imageEffect = GetComponent<ScreenTransitionImageEffect>() as ScreenTransitionImageEffect;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingForUserInput) {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) {
                waitingForUserInput = false;
                StartCoroutine(FadeIn());
            }
        }
    }

    public void FadeOut() {
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack() {
        if(!fading) {
            fading = true;
            float start = Time.time;
            float end = start + fadeDuration;
            imageEffect.Enable();
            imageEffect.maskColor = new Color(imageEffect.maskColor.r, imageEffect.maskColor.g, imageEffect.maskColor.b, 0);
            // Fade Out
            while(Time.time < end) {
                float percentage = 1 - (end - Time.time)/fadeDuration;
                imageEffect.maskValue = Mathf.Lerp(0,1, percentage);
                imageEffect.maskColor = new Color(imageEffect.maskColor.r, imageEffect.maskColor.g, imageEffect.maskColor.b, Mathf.Lerp(0,1,percentage));
                yield return null;
            }
            imageEffect.maskValue = 1;
            fadeOutComplete.Raise();
            waitingForUserInput = true;
            fading = false;
        }
    }


    IEnumerator FadeIn() {
        if(!fading) {
            fading = true;
            float start = Time.time;
            float end = start + fadeDuration;
            imageEffect.Enable();
            imageEffect.maskColor = new Color(imageEffect.maskColor.r, imageEffect.maskColor.g, imageEffect.maskColor.b, 0);
            // Fade Out
            while(Time.time < end) {
                float percentage = (end - Time.time)/fadeDuration;
                imageEffect.maskValue = Mathf.Lerp(0,1, percentage);
                imageEffect.maskColor = new Color(imageEffect.maskColor.r, imageEffect.maskColor.g, imageEffect.maskColor.b, Mathf.Lerp(0,1,percentage));
                yield return null;
            }
            imageEffect.maskValue = 0;
            fadeInComplete.Raise();
            fading = false;
        }
    }

}
