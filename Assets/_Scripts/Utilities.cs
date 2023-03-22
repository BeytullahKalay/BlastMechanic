using UnityEngine;

public static class Utilities
{
    public static void SetLocalPositionY(Transform t, float newYPos = 0)
    {
        var newLocalPos = t.localPosition;
        newLocalPos.y = newYPos;
        t.localPosition = newLocalPos;
    }

    public static void SetPositionY(Transform t, float newYPos = 0)
    {
        var newPos = t.position;
        newPos.y = newYPos;
        t.position = newPos;
    }

    public static float GetTopOfScreenY(Camera cam)
    {
        return cam.ViewportToWorldPoint(Vector3.one).y;
    }
}
