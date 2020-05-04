﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private static GameObject instance;

    private void Awake()
    {
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        int scene = SceneManager.GetActiveScene().buildIndex;
        if (scene == 0)
            Play("MainMenu");
        DontDestroyOnLoad(gameObject);
    }

    //===============PROCEDURE===============//
    public void Play(string name)
    //Purpose:          To play a specific audio clip
    //string name:      Finds the name of the audio
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //Error prevention
        if (s == null)
            Debug.LogError("Sound was not found, cannot play clip");
        else
            s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //Error prevention
        if (s == null)
            Debug.LogError("Sound was not found, cannot play clip");
        else
            s.source.Stop();

    }
}
