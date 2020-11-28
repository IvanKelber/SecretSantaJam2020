using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
public class StaticUserControls : MonoBehaviour
{

    public static bool paused = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            Time.timeScale = (Time.timeScale + 1) % 2;
            AudioListener.pause = paused;
        }
    }

    void Pause() {
        paused = !paused;
        Time.timeScale = (Time.timeScale + 1) % 2;
        AudioListener.pause = paused; 
    }
}
