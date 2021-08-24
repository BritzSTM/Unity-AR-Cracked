using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    [SerializeField] private EventTypeVoid _zoomInEventSO;
    [SerializeField] private EventTypeVoid _zoomOutEventSO;

    public float CriticalPoint = 2.0f;

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch one = Input.GetTouch(0);
            Touch two = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = one.position - one.deltaPosition;
            Vector2 touchOnePrevPos = two.position - two.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (one.position - two.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if(Mathf.Abs(deltaMagnitudeDiff) >= CriticalPoint)
            {
                if (deltaMagnitudeDiff < 0)
                    _zoomInEventSO.RaiseEvent();
                else
                    _zoomOutEventSO.RaiseEvent();
            }
        }
    }
}
