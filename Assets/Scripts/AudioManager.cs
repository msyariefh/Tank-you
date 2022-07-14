using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup mixer;
    [Range(0f, 1f)] public float volumeAnimation = 0f;
    private AudioSource currentAudioSource;
    private float normalVolume;
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound sound in sounds)
        {
            
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;
        }
        
    }
    private void Start()
    {
        PlaySound("MainMenu");
    }

    private void Update()
    {
        if(currentAudioSource == null) { return; }
        if(volumeAnimation >= 1)
        {
            currentAudioSource = null;
            return;
        }
        //Debug.Log(normalVolume);
        
        currentAudioSource.volume = normalVolume * volumeAnimation;
    }

    public void PlaySound(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audioName);
        if (s == null)
        {
            Debug.LogWarning("Audio: " + audioName + " Not Found!");
            return;
        }
        s.source.Play();
        if (s.isBGM) { currentAudioSource = s.source; }
        normalVolume = s.volume;
    }
    public void PlaySound(string prevAudioName, string nextAudioName)
    {
        Sound prev = Array.Find(sounds, sound => sound.name == prevAudioName);
        Sound next = Array.Find(sounds, sound => sound.name == nextAudioName);

        if (prev == null || next == null)
        {
            Debug.LogWarning("Audio: " + prevAudioName + " or " + nextAudioName + " Not Found!");
            return;
        }
        prev.source.Stop();
        next.source.Play();
        if (next.isBGM) { currentAudioSource = next.source; }
        normalVolume = next.volume;
    }

    public void AddEffectOnSound(string soundName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        sound.source.outputAudioMixerGroup = mixer;
    }

    public void RemoveEffectOnSound(string soundName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        sound.source.outputAudioMixerGroup = null;
    }
}
