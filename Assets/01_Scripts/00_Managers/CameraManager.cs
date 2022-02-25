using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public PostProcessProfile profile;

    [Header ("Shake")]
    public float duration;
    public float strength = 90;
    public int vibrato = 10;
    public float randomness = 90;

    [Space]
    [Header("Vignette")]
    [UnityEngine.Min(0.2f)]
    public float vignettageSpeed;
    public float baseVignettage;
    public float vignettageTarget = 0.66f;

    [Space]
    [Header("Zoom")]
    public float ZoomTarget = 0.66f;
    public float baseZoom = 25.17f;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : CameraManager");
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //EndSlowMoEffect();
    }

    public void ShakeCam()
    {
        transform.DOShakeRotation(duration, strength, vibrato, randomness);
    }

    public void BeginSlowMotion()
    {
        Camera.main.DOOrthoSize(ZoomTarget, 0.5f);
        StartCoroutine(StartSlowMoEffect());
    }

    public void EndSlowMotion()
    {
        Camera.main.DOOrthoSize(baseZoom, 0.5f);
        StartCoroutine(EndSlowMoEffect());
    }

    IEnumerator StartSlowMoEffect()
    {
        float time = 0;
        float vignettage = profile.GetSetting<Vignette>().intensity.value;
        while (time < vignettageSpeed)
        {
            float value = Mathf.Lerp(vignettage, vignettageTarget, time);
            time += Time.deltaTime;

            profile.GetSetting<Vignette>().intensity.value = value;
            yield return null;
        }
        profile.GetSetting<Vignette>().intensity.value = vignettageTarget;
    }

    IEnumerator EndSlowMoEffect()
    {
        float time = 0;
        float vignettage = profile.GetSetting<Vignette>().intensity.value;
        while (time < vignettageSpeed)
        {
            float value = Mathf.Lerp(vignettage, baseVignettage, time);
            time += Time.deltaTime;
            profile.GetSetting<Vignette>().intensity.value = value;
            yield return null;
        }
        profile.GetSetting<Vignette>().intensity.value = baseVignettage;
    }
}
