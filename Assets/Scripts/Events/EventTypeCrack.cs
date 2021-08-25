using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new crack event type so", menuName = "Game/Event/Crack")]
public class EventTypeCrack : ScriptableObject
{
    public event UnityAction<Crack> OnEvent;

    public void RaiseEvent(Crack crack)
    {
        if (OnEvent != null)
            OnEvent.Invoke(crack);
    }
}
