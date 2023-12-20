using UnityEngine;

public static class IsometricHelper
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    
    public static Vector3 ToIsometric(this Vector3 vector)
    {
        return _isoMatrix.MultiplyPoint3x4(vector);
    }
}
