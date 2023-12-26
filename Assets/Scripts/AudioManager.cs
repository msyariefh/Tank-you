using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    //public AudioMixerGroup mixer;
    //[Range(0f, 1f)] public float volumeAnimation = 0f;
    //private AudioSource currentAudioSource;
    //private float normalVolume;
    public static AudioManager Instance;

    private Dictionary<string, Sound> _database;
    private AudioSource _bgmAudioSource;
    private AudioSource _sfxAudioSource;
    [Header("New Components")]
    [SerializeField] private AudioMixerGroup _sfxMixer;
    [SerializeField] private AudioMixerGroup _bgmMixer;
      
    private void Awake()
    {
        if (Instance !=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Initialize();
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private float _fullAudio = 0f;
    private float _noAudio = -80f;
    private void Initialize()
    {
        

        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        _bgmAudioSource.outputAudioMixerGroup = _bgmMixer;
        _sfxAudioSource = gameObject.AddComponent<AudioSource>();
        _sfxAudioSource.outputAudioMixerGroup = _sfxMixer;

        _database = new();
        foreach (var snd in sounds)
        {
            if (_database.ContainsKey(snd.name)) continue;
            _database.Add(snd.name, snd);
        }
    }
    public bool CheckSettings(SoundMixType type)
    {
        switch (type)
        {
            case SoundMixType.BGM:
                if (!PlayerPrefs.HasKey("BGM"))
                {
                    PlayerPrefs.SetInt("BGM", 1);
                    return true;
                }
                else
                {
                    return IsTrue(PlayerPrefs.GetInt("BGM"));
                }
            case SoundMixType.SFX:
                if (!PlayerPrefs.HasKey("SFX"))
                {
                    PlayerPrefs.SetInt("SFX", 1);
                    return true;
                }
                else
                {
                    return IsTrue(PlayerPrefs.GetInt("SFX"));
                }
            default:
                return true;
        }
    }    
    public void SetSetting(SoundMixType type)
    {
        switch (type)
        {
            case SoundMixType.BGM:
                if (!PlayerPrefs.HasKey("BGM"))
                {
                    PlayerPrefs.SetInt("BGM", 0);
                    _bgmMixer.audioMixer.SetFloat("BGMVol", _noAudio);
                }
                else
                {
                    if (IsTrue(PlayerPrefs.GetInt("BGM")))
                    {
                        PlayerPrefs.SetInt("BGM", 0);
                        _bgmMixer.audioMixer.SetFloat("BGMVol", _noAudio);
                    }   
                    else
                    {
                        PlayerPrefs.SetInt("BGM", 1);
                        _bgmMixer.audioMixer.SetFloat("BGMVol", _fullAudio);
                    }
                    
                }
                break;
            case SoundMixType.SFX:
                if (!PlayerPrefs.HasKey("SFX"))
                {
                    PlayerPrefs.SetInt("SFX", 0);
                    _sfxMixer.audioMixer.SetFloat("SFXVol", _noAudio);
                }
                else
                {
                    if (IsTrue(PlayerPrefs.GetInt("SFX")))
                    {
                        PlayerPrefs.SetInt("SFX", 0);
                        _sfxMixer.audioMixer.SetFloat("SFXVol", _noAudio);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("SFX", 1);
                        _sfxMixer.audioMixer.SetFloat("SFXVol", _fullAudio);
                    }
                }
                break;
            default:
                break;
        }
    }
    private bool IsTrue(int val)
    {
        return val >= 1;
    }

    private void Start()
    {
        var bgmVolume = CheckSettings(SoundMixType.BGM) ? _fullAudio : _noAudio;
        var sfxVolume = CheckSettings(SoundMixType.SFX) ? _fullAudio : _noAudio;
        _bgmMixer.audioMixer.SetFloat("BGMVol", bgmVolume);
        _sfxMixer.audioMixer.SetFloat("SFXVol", sfxVolume);
        PlayBgmSound("MainMenu");
    }
    //public void ChangeVolumeSetting(SoundMixType type, bool isActive)
    //{
    //    switch (type)
    //    {
    //        case SoundMixType.BGM:
    //            _bgmMixer.audioMixer.SetFloat("BGMVol", isActive ? _fullAudio : _noAudio);
    //            break;
    //        case SoundMixType.SFX:
    //            _sfxMixer.audioMixer.SetFloat("SFXVol", isActive? _fullAudio : _noAudio);
    //            break;
    //    }
    //}

    //private void Update()
    //{
    //    if(currentAudioSource == null) { return; }
    //    if(volumeAnimation >= 1)
    //    {
    //        currentAudioSource = null;
    //        return;
    //    }
    //    //Debug.Log(normalVolume);
        
    //    currentAudioSource.volume = normalVolume * volumeAnimation;
    //}

    public void PlayBgmSound(string audioName)
    {
        try
        {
            var snd = _database[audioName] ?? 
                throw new KeyNotFoundException(audioName);

            _bgmAudioSource.clip = snd.clip;
            _bgmAudioSource.loop = snd.loop;
            _bgmAudioSource.volume = snd.volume;
            _affectedVolume = snd.volume;
            _bgmAudioSource.Play();

            //normalVolume = snd.volume;
            //currentAudioSource = _bgmAudioSource;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        //Sound s = Array.Find(sounds, sound => sound.name == audioName);
        //if (s == null)
        //{
        //    Debug.LogWarning("Audio: " + audioName + " Not Found!");
        //    return;
        //}
        //s.source.Play();
        //if (s.isBGM) { currentAudioSource = s.source; }
        //normalVolume = s.volume;
    }
    public void PlaySFX(string audioName)
    {
        try
        {
            var snd = _database[audioName] ??
                throw new KeyNotFoundException(audioName);

            _sfxAudioSource.volume = snd.volume;
            _sfxAudioSource.PlayOneShot(snd.clip);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    //public void PlaySound(string prevAudioName, string nextAudioName)
    //{
    //    Sound prev = Array.Find(sounds, sound => sound.name == prevAudioName);
    //    Sound next = Array.Find(sounds, sound => sound.name == nextAudioName);

    //    if (prev == null || next == null)
    //    {
    //        Debug.LogWarning("Audio: " + prevAudioName + " or " + nextAudioName + " Not Found!");
    //        return;
    //    }
    //    prev.source.Stop();
    //    next.source.Play();
    //    if (next.isBGM) { currentAudioSource = next.source; }
    //    normalVolume = next.volume;
    //}

    private float _affectedVolume = 0f;
    public void AddEffectOnSound()
    {
        //Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        //sound.source.outputAudioMixerGroup = mixer;
        _bgmAudioSource.volume = _affectedVolume / 2f;
        //_bgmMixer.audioMixer.SetFloat("BGMPitch", .8f);

    }

    public void RemoveEffectOnSound()
    {
        _bgmAudioSource.volume = _affectedVolume;
        //_bgmMixer.audioMixer.SetFloat("BGMPitch", 1);
    }
}
public enum SoundMixType
{
    BGM,
    SFX
}