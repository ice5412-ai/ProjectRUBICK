using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBox : MonoBehaviour
{
    [SerializeField] public AudioClip ClickingSFX;
    [SerializeField] public AudioClip Music;

    private void Start()
    {
        SoundManagerSingleton.Instance.Unmute();
        if (SoundManagerSingleton.Instance.musicSource.clip != Music)
        {
            SoundManagerSingleton.Instance.PlayMusic(Music);
        }
    }

    public void Clicking()
    {
        SoundManagerSingleton.Instance.PlaySFX(ClickingSFX);
    }
    
    public void MuteOrUnmute()
    {
        SoundManagerSingleton.Instance.MuteOrUnmute();
    }

    public void ChangeMasterVol(Slider slider)
    {
        SoundManagerSingleton.Instance.ChangeMasterVol(slider.value);
    }
    
    public void ChangeMusicVol(Slider slider)
    {
        SoundManagerSingleton.Instance.ChangeMusicVol(slider.value);
    }
    
    public void ChangeSFXVol(Slider slider)
    {
        SoundManagerSingleton.Instance.ChangeSFXVol(slider.value);
    }
}
