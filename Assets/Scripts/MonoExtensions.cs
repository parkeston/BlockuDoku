using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoExtensions
{
    private static readonly Vector3[] Corners = new Vector3[4];
    public static void GetBounds(this RectTransform rectTransform,ref Bounds bounds)
    {
        rectTransform.GetWorldCorners(Corners);
        bounds.SetMinMax(Corners[0],Corners[2]);
    }
}
