using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new game object event type", menuName = "Game/Event/GameObject")]
public class EventTypeGameObject : ScriptableObject
{
    public event UnityAction<GameObject> OnEvent;

    public void RaiseEvent(GameObject obj)
    {
        if (OnEvent != null)
            OnEvent.Invoke(obj);
    }
}
