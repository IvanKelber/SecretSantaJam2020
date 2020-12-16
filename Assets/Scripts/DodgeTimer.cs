using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeTimer : MonoBehaviour
{

    [SerializeField]
    Slider slider;

    [SerializeField]
    PlayerValues playerValues;
  
    // Update is called once per frame
    void Update()
    {
        slider.maxValue = playerValues.dodgeCooldown;
    }

    public void UpdateTimer(float value) {
        slider.value = Mathf.Clamp(value, 0, slider.maxValue);
    }
}
