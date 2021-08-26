using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new game manager event", menuName = "Game/Event/GameManager")]
public class EventTypeGameManager : ScriptableObject
{
    public event UnityAction<GameManager> OnEvent;

    public void RaiseEvent(GameManager manager)
    {
        if (OnEvent != null)
            OnEvent.Invoke(manager);
    }
}
