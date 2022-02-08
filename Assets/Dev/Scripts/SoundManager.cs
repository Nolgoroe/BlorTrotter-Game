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

public class SoundManager : MonoBehaviour, IManageable
{
    public static SoundManager instance;

    public AudioSource audioSource;

    public EnumAndClip[] soundsForGame;

    public Dictionary<Sounds, AudioClip> enumToSound;



    public void initManager()
    {
        instance = this;


        audioSource = GetComponent<AudioSource>();


        enumToSound = new Dictionary<Sounds, AudioClip>();


        foreach (EnumAndClip enumAndClip in soundsForGame)
        {
            enumToSound.Add(enumAndClip.enumSound, enumAndClip.theSoundClip);
        }

        Debug.Log("success Sound");
    }

    public void PlaySound(Sounds soundEnum)
    {
        audioSource.PlayOneShot(enumToSound[soundEnum]);
    }

    public void PlaySoundChangeVolume(Sounds soundEnum, float Volume)
    {
        audioSource.volume = Volume;

        audioSource.PlayOneShot(enumToSound[soundEnum]);
    }

    public async void PlaySoundDelay(Sounds soundEnum, int timeToWait)
    {
        await Task.Delay(timeToWait * 1000);

        audioSource.PlayOneShot(enumToSound[soundEnum]);
    }

    public async void PlaySoundChangeVolumeAndDelay(Sounds soundEnum, float Volume, int timeToWait)
    {
        await Task.Delay(timeToWait * 1000);

        audioSource.volume = Volume;

        audioSource.PlayOneShot(enumToSound[soundEnum]);
    }

    public void ResetVolume()
    {
        audioSource.volume = 1;
    }
}




