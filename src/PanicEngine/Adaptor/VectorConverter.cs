using UnityEngine;
using PanicEngine.Core;
using PanicEngine.Maths;

public static class VectorConverter
{
    public static Vector2 ToUnityVector2(Vector2D vector2D)
    {
        return new Vector2(vector2D.X, vector2D.Y);
    }

    public static Vector2D ToPanicVector2D(Vector2 vector2)
    {
        return new Vector2D(vector2.x, vector2.y);
    }

    public static Vector2D FromVector3ToVector2D(Vector3 vector3)
    {
        return new Vector2D(vector3.x, vector3.y);
    }
}
