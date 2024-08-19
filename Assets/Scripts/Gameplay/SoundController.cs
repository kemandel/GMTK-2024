using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public List<AudioSource> AudioSources { get; private set; }

    public AudioSource PlaySound(AudioClip audioClip, float pitch = 1, float volume = 1)
    {
        //loop through all audiosources and see if they are playing
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        int i;
        for (i = 0; i < audioSources.Length; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                //found an audio source that is free
                audioSources[i].clip = audioClip;
                audioSources[i].pitch = pitch;
                audioSources[i].volume = volume;
                audioSources[i].Play();
                return audioSources[i];
            }

        }

        //at this point, no audiosources were free, so make a new one
        gameObject.AddComponent<AudioSource>();
        AudioSource newSource = GetComponentsInChildren<AudioSource>()[i];
        newSource.loop = false;
        newSource.playOnAwake = false;
        newSource.pitch = pitch;
        newSource.volume = volume;
        newSource.clip = audioClip;
        newSource.Play();
        return newSource;
    }
}
