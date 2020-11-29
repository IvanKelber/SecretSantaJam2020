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

    void Pause(bool toPause) {
        paused = toPause;
        Time.timeScale = paused ? 0 : 1;
        AudioListener.pause = paused; 
    }

    void OpenMenu() {
        Pause(true);
    }

    void CloseMenu() {
        Pause(false);
    }
}
