using UnityEngine;
using TMPro;

public abstract class GMTextUpdaterBase : MonoBehaviour
{
    [SerializeField] private EventTypeGameManager _onGameManagerUpdateSO;
    [SerializeField] private string _prefixStr;
    private TMP_Text _text;

    private void Awake()
    {
        if (_onGameManagerUpdateSO == null)
            Debug.LogWarning($"[{nameof(GMTextUpdaterBase)}][{gameObject.name}] event so null");

        _text = GetComponent<TMP_Text>();
        Debug.Assert(_text != null);
    }

    protected virtual void OnEnable()
    {
        _onGameManagerUpdateSO.OnEvent += OnUpdateText;
    }

    protected virtual void OnDisable()
    {
        _onGameManagerUpdateSO.OnEvent -= OnUpdateText;
    }

    protected void SetText(string str)
    {
        _text.text = string.Format("{0} {1}", _prefixStr, str);
    }

    protected abstract void OnUpdateText(GameManager manager);
}
