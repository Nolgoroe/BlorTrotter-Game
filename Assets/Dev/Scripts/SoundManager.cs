using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public enum Sounds
{
    Blob_Eat_Absorb,
    Blob_Hurt,
    Blob_Moving_Spawning,
    Slug_Moving,
    Slug_Spawning,
    Lose,
    Selection,
    Victory,
    OneStar,
    TwoStar,
    ThreeStar,
    MenuMusic,
    Beetle_Eat,
    Beetle_Flight,
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


        enumToSound = new Dictionary<Sounds, AudioClip>();


        foreach (EnumAndClip enumAndClip in soundsForGame)
        {
            enumToSound.Add(enumAndClip.enumSound, enumAndClip.theSoundClip);
        }


        canHearMusic = true;
        canHearSounds = true;

        Debug.Log("success Sound");
    }

    public void PlaySound(AudioSource source, Sounds soundEnum)
    {
        //source.PlayOneShot(enumToSound[soundEnum]);
        source.Stop();

        source.PlayOneShot(enumToSound[soundEnum]);
    }
    public void PlaySoundFadeOut(AudioSource source, Sounds soundEnum)
    {
        //source.PlayOneShot(enumToSound[soundEnum]);

        source.PlayOneShot(enumToSound[soundEnum]);

        float originalVolume = SFXAudioSource.volume;

        LeanTween.value(SFXAudioSource.gameObject, SFXAudioSource.volume, 0, 2.6f).setOnComplete(() => SFXAudioSource.volume = originalVolume).setOnUpdate((float val) =>
        {
            SFXAudioSource.volume = val;
        });
    }
    public void PlaySoundAsClip(AudioSource source, AudioClip SFX, bool isLooping)
    {
        if (isLooping)
        {
            source.loop = true;
        }

        source.clip = SFX;

        source.Play();
    }

    public void PlayMusic(AudioSource source, AudioClip music)
    {
        source.clip = music;

        source.Play();
    }

    public void PlaySoundChangeVolume(AudioSource source, Sounds soundEnum, float Volume)
    {
        source.volume = Volume;

        source.PlayOneShot(enumToSound[soundEnum]);
    }

    public async void PlaySoundDelay(AudioSource source, Sounds soundEnum, int timeToWait)
    {
        await Task.Delay(timeToWait * 1000);

        source.PlayOneShot(enumToSound[soundEnum]);
    }

    public async void PlaySoundChangeVolumeAndDelay(AudioSource source, Sounds soundEnum, float Volume, int timeToWait)
    {
        await Task.Delay(timeToWait * 1000);

        source.volume = Volume;

        source.PlayOneShot(enumToSound[soundEnum]);
    }

    public void ResetVolume()
    {
        SFXAudioSource.volume = 1;
        musicAudioSource.volume = 1;
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void UIInteractSound()
    {
        if (canHearSounds)
        {
            PlaySound(SFXAudioSource, Sounds.Selection);
        }
    }
}




