using UnityEngine;

public class GunFireTriggerForSO : MonoBehaviour
{
    [SerializeField] private EventTypeVoid _leftGunFireSO;
    [SerializeField] private EventTypeVoid _RightGunFireSO;

    private void Awake()
    {
        Debug.Assert(_leftGunFireSO != null && _RightGunFireSO != null);
    }

    public void FireLeftGun()
    {
        _leftGunFireSO?.RaiseEvent();
    }

    public void FireRightGun()
    {
        _RightGunFireSO?.RaiseEvent();
    }
}
