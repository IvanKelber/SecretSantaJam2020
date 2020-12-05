using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Armor : MonoBehaviour
{
    public Slider slider;

    public PlayerValues playerValues;

    public void Update() {
        slider.maxValue = playerValues.maxArmor;
        slider.value = playerValues.currentArmor;    
    }
}
