using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSfxSource;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one instance of AudioManager found!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        musicSource.loop = true;
        loopSfxSource.loop = true;
        sfxSource.loop = false;
    }


    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void PlaySound(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        loopSfxSource.volume = value;
    }


    public void PlaySoundLooped(AudioClip clip)
    {
        if (clip == null) return;
        loopSfxSource.clip = clip;
        loopSfxSource.loop = true;
        loopSfxSource.Play();
    }

    public void StopLoopedSound()
    {
        loopSfxSource.Stop();
        loopSfxSource.clip = null;
    }
}