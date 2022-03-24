using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public enum Sounds
{
    //// Add all sounds here
}


[System.Serializable]
public class EnumAndClip
{
    public Sounds enumSound;

    public AudioClip theSoundClip;
}

public class SoundManager : MonoBehaviour, IManageable   //singleton , only instantiate one time 
{
    public static SoundManager instance;

    public AudioSource musicAudioSource;
    public AudioSource SFXAudioSource;

    public EnumAndClip[] soundsForGame;

    public Dictionary<Sounds, AudioClip> enumToSound;


    public bool canHearMusic, canHearSounds;


    public void initManager()
    {
        instance = this;


        SFXAudioSource = GetComponent<AudioSource>();


        enumToSound = new Dictionary<Sounds, AudioClip>();


        foreach (EnumAndClip enumAndClip in soundsForGame)
        {
            enumToSound.Add(enumAndClip.enumSound, enumAndClip.theSoundClip);
        }


        canHearMusic = true;
        canHearSounds = true;

        Debug.Log("success Sound");
    }

    public void PlaySound(Sounds soundEnum)
    {
        SFXAudioSource.PlayOneShot(enumToSound[soundEnum]);
    }

    public void PlaySoundChangeVolume(Sounds soundEnum, float Volume)
    {
        SFXAudioSource.volume = Volume;

        SFXAudioSource.PlayOneShot(enumToSound[soundEnum]);
    }

    public async void PlaySoundDelay(Sounds soundEnum, int timeToWait)
    {
        await Task.Delay(timeToWait * 1000);

        SFXAudioSource.PlayOneShot(enumToSound[soundEnum]);
    }

    public async void PlaySoundChangeVolumeAndDelay(Sounds soundEnum, float Volume, int timeToWait)
    {
        await Task.Delay(timeToWait * 1000);

        SFXAudioSource.volume = Volume;

        SFXAudioSource.PlayOneShot(enumToSound[soundEnum]);
    }

    public void ResetVolume()
    {
        SFXAudioSource.volume = 1;
    }
}




