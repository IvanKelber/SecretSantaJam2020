using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    void OnInteract();

    void OnEnterTrigger(Collider2D collider);

    void OnStayTrigger(Collider2D collider);
    void OnExitTrigger(Collider2D collider);
}
