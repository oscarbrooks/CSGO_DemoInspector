using DemoInfo;
using UnityEngine;

public class DemoInfoHelper {
    public static Vector3 ViewAnglesToVector3(float angleX, float angleY)
    {
        var lineDistance = 1;
        var dx = lineDistance * Mathf.Sin(Mathf.PI * (angleX + 90) / 180);
        var dy = lineDistance * Mathf.Sin(Mathf.PI * angleY / 180);
        var dz = lineDistance * Mathf.Cos(Mathf.PI * (angleX + 90) / 180);
        var vector = new Vector3(-dx, -dy, dz);
        vector.Normalize();
        return vector;
    }

    public static Vector3 SourceToUnityVector(Vector sourceVector)
    {
        return new Vector3(-sourceVector.X, sourceVector.Z, -sourceVector.Y);
    }
}
