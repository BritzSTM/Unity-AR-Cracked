using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new void event type", menuName = "Game/Event/Void")]
public class EventTypeVoid : ScriptableObject
{
    public event UnityAction OnEvent;

    public void RaiseEvent()
    {
        if (OnEvent != null)
            OnEvent.Invoke();
    }
}
