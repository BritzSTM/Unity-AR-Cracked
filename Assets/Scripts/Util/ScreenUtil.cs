using UnityEngine;

public static class ScreenUtil
{
    private static Rect _cachedRect = new Rect();
    private static Rect _cachedScreenRect = new Rect();
    private static Vector3[] _cachedVectors = new Vector3[32];

    public static void GetScreenSpaceRectFromBounds(this GameObject obj, Camera camera, ref Rect res)
    {
        var objBounds = obj.GetComponent<Renderer>().bounds;
        if (objBounds == null)
        {
            Debug.LogWarning($"{obj.name} not found bounds from renderer");
            res.x = res.y = 0;
            res.width = res.height = 0;

            return;
        }

        _cachedVectors[0].x = objBounds.center.x - objBounds.extents.x;
        _cachedVectors[0].y = objBounds.center.y + objBounds.extents.y;
        _cachedVectors[0].z = objBounds.center.z + objBounds.extents.z;
        _cachedVectors[1] = camera.WorldToScreenPoint(_cachedVectors[0]);

        _cachedVectors[0].x = objBounds.center.x + objBounds.extents.x;
        _cachedVectors[0].y = objBounds.center.y + objBounds.extents.y;
        _cachedVectors[0].z = objBounds.center.z + objBounds.extents.z;
        _cachedVectors[2] = camera.WorldToScreenPoint(_cachedVectors[0]);

        _cachedVectors[0].x = objBounds.center.x - objBounds.extents.x;
        _cachedVectors[0].y = objBounds.center.y - objBounds.extents.y;
        _cachedVectors[0].z = objBounds.center.z + objBounds.extents.z;
        _cachedVectors[3] = camera.WorldToScreenPoint(_cachedVectors[0]);

        _cachedVectors[0].x = objBounds.center.x + objBounds.extents.x;
        _cachedVectors[0].y = objBounds.center.y - objBounds.extents.y;
        _cachedVectors[0].z = objBounds.center.z + objBounds.extents.z;
        _cachedVectors[4] = camera.WorldToScreenPoint(_cachedVectors[0]);

        _cachedVectors[0].x = objBounds.center.x - objBounds.extents.x;
        _cachedVectors[0].y = objBounds.center.y + objBounds.extents.y;
        _cachedVectors[0].z = objBounds.center.z - objBounds.extents.z;
        _cachedVectors[5] = camera.WorldToScreenPoint(_cachedVectors[0]);

        _cachedVectors[0].x = objBounds.center.x + objBounds.extents.x;
        _cachedVectors[0].y = objBounds.center.y + objBounds.extents.y;
        _cachedVectors[0].z = objBounds.center.z - objBounds.extents.z;
        _cachedVectors[6] = camera.WorldToScreenPoint(_cachedVectors[0]);

        _cachedVectors[0].x = objBounds.center.x - objBounds.extents.x;
        _cachedVectors[0].y = objBounds.center.y - objBounds.extents.y;
        _cachedVectors[0].z = objBounds.center.z - objBounds.extents.z;
        _cachedVectors[7] = camera.WorldToScreenPoint(_cachedVectors[0]);

        _cachedVectors[0].x = objBounds.center.x + objBounds.extents.x;
        _cachedVectors[0].y = objBounds.center.y - objBounds.extents.y;
        _cachedVectors[0].z = objBounds.center.z - objBounds.extents.z;
        _cachedVectors[8] = camera.WorldToScreenPoint(_cachedVectors[0]);

        res.xMin = _cachedVectors[1].x;
        res.yMin = _cachedVectors[1].y;
        res.xMax = _cachedVectors[1].x;
        res.yMax = _cachedVectors[1].y;

        for (int i = 2; i <= 8; ++i)
        {
            if (res.xMin > _cachedVectors[i].x)
                res.xMin = _cachedVectors[i].x;

            if (res.yMin > _cachedVectors[i].y)
                res.yMin = _cachedVectors[i].y;

            if (res.xMax < _cachedVectors[i].x)
                res.xMax = _cachedVectors[i].x;

            if (res.yMax < _cachedVectors[i].y)
                res.yMax = _cachedVectors[i].y;
        }
    }

    /// <summary>
    /// 해당 카메라를 이용한 변환시 화면에 보이는가 대한 질의
    /// 일부분 만이라도 보이게 된다면 보이는 걸로 판정
    /// </summary>
    public static bool IsVisibleInScreen(this GameObject obj, Camera camera, ref Rect screenRect)
    {
        obj.GetScreenSpaceRectFromBounds(camera, ref _cachedRect);

        if (screenRect != null)
            screenRect = _cachedRect;

        _cachedScreenRect.xMin = 0;
        _cachedScreenRect.yMin = 0;
        _cachedScreenRect.xMax = Screen.width;
        _cachedScreenRect.yMax = Screen.height;

        return _cachedScreenRect.Overlaps(_cachedRect);
    }
}
