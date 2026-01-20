using UnityEngine;
using PanicEngine.Core;
using PanicEngine.Maths;

public static class Vector2DUnity
{
    public static Vector2 ToUnity(this Vector2D vector2D)
    {
        return new Vector2(vector2D.X, vector2D.Y);
    }

    public static Vector2D ToPanic(this Vector2 vector2)
    {
        return new Vector2D(vector2.x, vector2.y);
    }
}
