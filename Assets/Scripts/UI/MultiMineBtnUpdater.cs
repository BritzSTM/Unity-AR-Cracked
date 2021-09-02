using UnityEngine;
using UnityEngine.UI;

public class MultiMineBtnUpdater : MonoBehaviour
{
    [SerializeField] private EventTypeGameManager _onUpdateMinedCountSO;
    [SerializeField] private Button _targetBtn;

    private void OnEnable()
    {
        _onUpdateMinedCountSO.OnEvent += OnUpdateMinedCount;
    }

    private void OnDisable()
    {
        _onUpdateMinedCountSO.OnEvent -= OnUpdateMinedCount;
    }

    private void OnUpdateMinedCount(GameManager manager)
    {
        if (manager.MinedDimCount >= manager.MultiMineGunCost)
            _targetBtn.interactable = true;
        else
            _targetBtn.interactable = false;
    }
}
