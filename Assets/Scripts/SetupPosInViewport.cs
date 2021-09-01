using UnityEngine;

/// <summary>
/// 초기화 시점에 카메라의 뷰포트 기준으로 객체를 이동시킵니다.
/// </summary>
public class SetupPosInViewport : MonoBehaviour
{
    [SerializeField] private Camera _pivotCam;
    [SerializeField] private Vector2 _targetPos;

    private void Awake()
    {
        if(_pivotCam == null)
        {
            Debug.LogWarning($"[{nameof(SetupPosInViewport)}]Not found pivot cam");
            return;
        }

        transform.position = _pivotCam.ViewportToWorldPoint(
            new Vector3(_targetPos.x, _targetPos.y, _pivotCam.nearClipPlane));
    }
}
