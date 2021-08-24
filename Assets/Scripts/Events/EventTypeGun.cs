using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct GunEventArgs
{
    public GameObject hit;
}

[CreateAssetMenu(fileName = "new gun event type", menuName = "Game/Event/Gun")]
public class EventTypeGun : ScriptableObject
{
    public event UnityAction<GunEventArgs> OnEvent;

    public void RaiseEvent(GunEventArgs args)
    {
        if (OnEvent != null)
            OnEvent.Invoke(args);
    }
}
