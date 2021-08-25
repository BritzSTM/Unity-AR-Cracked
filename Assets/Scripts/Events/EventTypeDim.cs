using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new DimObject event type so", menuName = "Game/Event/Dim")]
public class EventTypeDim : ScriptableObject
{
    public event UnityAction<DimObject> OnEvent;

    public void RaiseEvent(DimObject crack)
    {
        if (OnEvent != null)
            OnEvent.Invoke(crack);
    }
}
