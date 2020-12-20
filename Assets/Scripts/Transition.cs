using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ScriptableObjectArchitecture;
using Cinemachine;

public class Transition : MonoBehaviour
{
    [Range(0,2)]
    public float leftDuration = .5f;

    [Range(0,2)]
    public float rightDuration = .5f;

    [Range(0,2)]
    public float coverDuration = .5f;

    [Range(0,2)]
    public float slideOutDuration = .5f;

    public float raiseAmount = 700;

    public Image left;
    public Image right;

    public AudioManager audioManager;
    AudioSource audioSource;
    CinemachineImpulseSource impulseSource;

    public GameEvent coveredEvent; //During this event we generate the next level, move the player, adjust the camera, etc.

    Vector2 leftAnchor;
    Vector2 rightAnchor;

    bool transitioning = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        leftAnchor = left.rectTransform.anchoredPosition;
        rightAnchor = right.rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            StartTransition();
        }
    }

    public void StartTransition() {
        if(!transitioning) {
            StartCoroutine(DoTransition());
        }
    }

    IEnumerator DoTransition() {
        transitioning = true;
        //Slide in both sides
        StaticUserControls.Pause(true);
        Tween leftSlide = left.rectTransform.DOAnchorPos(Vector2.zero, leftDuration).SetUpdate(true);
        yield return leftSlide.WaitForCompletion();

        audioManager.Play("BulletBirth", audioSource);

        Tween rightSlide = right.rectTransform.DOAnchorPos(Vector2.zero, rightDuration).SetUpdate(true);
        yield return rightSlide.WaitForCompletion();
        audioManager.Play("BulletBirth", audioSource);

        yield return null; // Wait one frame before calling covered event
        coveredEvent.Raise();
        // Wait for some duration
        StaticUserControls.Pause(false);

        yield return new WaitForSecondsRealtime(coverDuration);
        // Slide out both sides
        Vector2 raise = new Vector2(0, raiseAmount);
        left.rectTransform.DOAnchorPos(raise, slideOutDuration).SetUpdate(true);
        Tween rightExit = right.rectTransform.DOAnchorPos(raise, slideOutDuration).SetUpdate(true);
        yield return rightExit.WaitForCompletion();
        left.rectTransform.anchoredPosition = leftAnchor;
        right.rectTransform.anchoredPosition = rightAnchor;
        transitioning = false;
    }
}
