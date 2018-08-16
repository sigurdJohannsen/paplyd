using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseClasses;

public class AudioManager : BaseSingleton<AudioManager>{

    public List<AudioClip> audioClips;
    private AudioSource audio;
    public AudioClip GetAudio(Sounds sound)
    {
        return audioClips[(int)sound];
    }
    public void PlayAudio(Sounds sound)
    {
        if (!audio)
            audio = GetComponent<AudioSource>();
        audio.clip = audioClips[(int)sound];
        audio.Play();
    }
    
}
public enum Sounds
{
    GrabSound,
    HoverSound,
    DropSound,
    MenuButton,
    Ambient_sub,
    ReturnSound,
    Ambient_garage,
}