/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private static GameObject instance;
    public bool regularAudio;
    public bool startAtBoss;
    public AudioMixerGroup mixer;

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
            s.source.outputAudioMixerGroup= mixer;
        }
        int scene = SceneManager.GetActiveScene().buildIndex;
        if (scene == 0)
            Play("MainMenu");
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        //If in a regular level, play regular level music
        if(scene == 1 && !regularAudio)
        {
            Play("RegularLevel");
            Stop("Victory");
            Stop("MainMenu");
            regularAudio = true;
        }
        //If in a boss level after coming from a regular level, stop regular and play boss
        else if(scene == 2 && regularAudio)
        {
            Stop("RegularLevel");
            Play("Boss");
            regularAudio = false;
        }
        else if(scene == 0)
        {
            regularAudio = false;
        }
        //Special case for testing
        if(startAtBoss && regularAudio)
        {
            Play("Boss");
            regularAudio = false;
            startAtBoss = false;
        }
    }
    
    public void Reset()
    {
        regularAudio = false;
    }

    //===============PROCEDURE===============//
    public void Play(string name)
    //Purpose:          To play a specific audio clip
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //Error prevention
        if (s == null)
            Debug.LogError("Sound was not found, cannot play clip");
        else
            s.source.Play();
    }

    //===============PROCEDURE===============//
    public void Stop(string name)
    //Purpose:          To stop a specific audio clip
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //Error prevention
        if (s == null)
            return;
        else
            s.source.Stop();
    }
}
