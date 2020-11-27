using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EndPortal : Interactable
{
    public GameEvent levelComplete;

    public override void OnInteract() {
        levelComplete.Raise();
    }
}
