using UnityEngine;

public class GameoverSetActiveAgent : MonoBehaviour
{
    [SerializeField] private EventTypeGameManager _eventSO;
    [SerializeField] private bool _targetState = false;
    [SerializeField] private GameObject[] targets;

    private void Awake()
    {
        Debug.Assert(_eventSO != null);
    }

    private void OnEnable()
    {
        _eventSO.OnEvent += OnEvent;
    }

    private void OnDisable()
    {
        _eventSO.OnEvent -= OnEvent;
    }

    protected void OnEvent(GameManager manager)
    {
        for(int i = 0; i < targets.Length; ++i)
        {
            targets[i].SetActive(_targetState);
        }
    }
}
