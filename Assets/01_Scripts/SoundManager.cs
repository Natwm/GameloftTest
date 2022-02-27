using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource _Audio;

    public AudioClip uiClick;
    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : SoundManager");
        instance = this;
    }

    private void Start()
    {
        _Audio = GetComponent<AudioSource>();
    }

    public static AudioClip GetRandomSound(List<AudioClip> sounds)
    {
        int index = Random.Range(0, sounds.Count);
        return sounds[index];
    }

    public void PlaySound(AudioClip clip)
    {
        _Audio.clip = clip;
        _Audio.Play();
    }

    public void PlayClickUI()
    {
        _Audio.clip = uiClick;
        _Audio.Play();
    }
}
