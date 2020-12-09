using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BootLoader 
{
    [RuntimeInitializeOnLoadMethod]
    private static void SetTargetFrameRate()
    {
        Application.targetFrameRate = 60;
    }
}
