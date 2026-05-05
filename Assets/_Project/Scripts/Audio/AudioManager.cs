using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one instance of AudioManager found!");
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    private void Start()
    {
        musicSource.loop = true;
        sfxSource.loop = false;
    }


    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
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
    }
}
