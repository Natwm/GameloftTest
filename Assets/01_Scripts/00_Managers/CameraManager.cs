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
    [Header("Chromatic")]
    public float baseChromatics = 0;
    public float chromaticsTarget = 0.66f;

    [Space]
    [Header("Zoom")]
    public float ZoomTarget = 0.66f;
    public float baseZoom = 25.17f;
    public Vector3 m_IntialPosition;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : CameraManager");
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_IntialPosition = transform.position;
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

    public void Zoom(float zoomValue, float zoomSpeed)
    {
        Camera.main.DOOrthoSize(zoomValue, zoomSpeed);
    }

    public void ResetZoom(float zoomSpeed)
    {
        Camera.main.DOOrthoSize(baseZoom, zoomSpeed);
    }

    public IEnumerator ZoomToward(Vector2 pos,float zoomValue, float zoomSpeed)
    {
        Vector3 position = new Vector3(pos.x, pos.y, transform.position.z);

        transform.DOMove(position, zoomSpeed);
        Camera.main.DOOrthoSize(zoomValue, zoomSpeed);

        yield return new WaitForSeconds(zoomSpeed);
        
        CameraManager.instance.ResetPositionAndZoom();
    }

    public void ResetPositionAndZoom()
    {
        transform.DOMove(m_IntialPosition, 0.2f);
        Camera.main.DOOrthoSize(baseZoom, 0.2f);
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
        float chromatics = profile.GetSetting<ChromaticAberration>().intensity.value;

        while (time < vignettageSpeed)
        {
            float vignettageValue = Mathf.Lerp(vignettage, vignettageTarget, time);
            float chromaticsValue = Mathf.Lerp(chromatics, chromaticsTarget, time);

            time += Time.deltaTime;

            profile.GetSetting<Vignette>().intensity.value = vignettageValue;
            profile.GetSetting<ChromaticAberration>().intensity.value = chromaticsValue;
            yield return null;
        }
        profile.GetSetting<Vignette>().intensity.value = vignettageTarget;
        profile.GetSetting<ChromaticAberration>().intensity.value = chromaticsTarget;
    }

    IEnumerator EndSlowMoEffect()
    {
        float time = 0;
        float vignettage = profile.GetSetting<Vignette>().intensity.value;
        float chromatics = profile.GetSetting<ChromaticAberration>().intensity.value;

        while (time < vignettageSpeed)
        {
            float value = Mathf.Lerp(vignettage, baseVignettage, time);
            float chromaticsValue = Mathf.Lerp(chromatics, baseChromatics, time);

            time += Time.deltaTime;
            profile.GetSetting<Vignette>().intensity.value = value;
            profile.GetSetting<ChromaticAberration>().intensity.value = chromaticsValue;
            yield return null;
        }
        profile.GetSetting<Vignette>().intensity.value = baseVignettage;
        profile.GetSetting<ChromaticAberration>().intensity.value = baseChromatics;
    }
}
