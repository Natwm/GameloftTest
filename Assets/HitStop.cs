using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop instance;

    bool waiting;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : HitStop");
        instance = this;
    }

    public void FreezeFrame(float duration)
    {
        if (waiting)
            return;

        CameraManager.instance.Zoom(20, 0.3f);
        Time.timeScale = 0.1f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;

        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;

        waiting = false;

        CameraManager.instance.ResetZoom(0.3f);
    }
}
