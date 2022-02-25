using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Limiter : MonoBehaviour
{
    public const int FPS_LIMITER = 60;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPS_LIMITER;
    }

    void Update()
    {
        if (Application.targetFrameRate > FPS_LIMITER)
            Application.targetFrameRate = FPS_LIMITER;
    }
}
