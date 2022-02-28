using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource _Audio;
    [SerializeField] private AudioSource _AudioAccord;

    [Space]
    public AudioClip uiClick;
    public List<AudioClip> UiSpawn;
    public List<AudioClip> accord;

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
    public void PlayStartSpawnUI()
    {
        _Audio.clip = GetRandomSound(UiSpawn);
        _Audio.Play();
    }

    public void PlayAccord()
    {
        _AudioAccord.clip = GetRandomSound(accord);
        _AudioAccord.Play();
    }
}
