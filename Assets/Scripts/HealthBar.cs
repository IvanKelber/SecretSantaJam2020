using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;

    public CanvasGroup healthBarGroup;

    public Image fill;

    public bool alwaysVisible = true;

    [Range(.5f, 5)]
    public float fullVisibility = 1;

    public void Start() {
        if(!alwaysVisible) {
            healthBarGroup.alpha = 0;
        }
    }

    public void SetMaxHealth(float maxHealth) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetCurrentHealth(float currentHealth) {
        StopCoroutine("FadeVisibility");
        healthBarGroup.alpha = 1;
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(!alwaysVisible) {
            if(fill.color != gradient.Evaluate(0)) {
                StartCoroutine("FadeVisibility");
            }
        }
    }

    public IEnumerator FadeVisibility() {
        yield return new WaitForSeconds(fullVisibility);

        healthBarGroup.alpha = 0;
    }
}
