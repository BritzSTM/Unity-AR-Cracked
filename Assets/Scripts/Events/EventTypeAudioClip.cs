using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new audio clip event type", menuName = "Game/Event/AudioClip")]
public class EventTypeAudioClip : ScriptableObject
{
    public event UnityAction<AudioClip> OnEvent;

    public void RaiseEvent(AudioClip audioClip)
    {
        if (OnEvent != null)
            OnEvent.Invoke(audioClip);
    }
}
