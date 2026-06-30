using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerSingleton : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioMixer mixer;
    
    protected SoundManagerSingleton() { }

    public static SoundManagerSingleton Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void ChangeMasterVol(float value)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
    }
    
    public void ChangeMusicVol(float value)
    {
        mixer.SetFloat("BGMVol", Mathf.Log10(value) * 20);
    }
    
    public void ChangeSFXVol(float value)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }

    public void MuteOrUnmute()
    {
        musicSource.mute = !musicSource.mute;
    }
    
    public void Unmute()
    {
        musicSource.mute = false;
    }
}
