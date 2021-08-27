using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new crack manager event type so", menuName = "Game/Event/CrackManager")]
public class EventTypeCrackManager : ScriptableObject
{
    public event UnityAction<CrackManager> OnEvent;

    public void RaiseEvent(CrackManager crackManager)
    {
        if (OnEvent != null)
            OnEvent.Invoke(crackManager);
    }
}
