using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip arSceneMusic;
    
    [Header("SFX Clips")]
    public AudioClip buttonClickSound;
    public AudioClip animalPlacedSound;
    public AudioClip animalSelectedSound;
    
    [Header("Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;
    
    private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeAudio()
    {
        // Create audio sources if not assigned
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        
        // Set initial volumes
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
        
        // Register sound effects
        RegisterSoundEffect("ButtonClick", buttonClickSound);
        RegisterSoundEffect("AnimalPlaced", animalPlacedSound);
        RegisterSoundEffect("AnimalSelected", animalSelectedSound);
    }
    
    private void RegisterSoundEffect(string name, AudioClip clip)
    {
        if (clip != null && !soundEffects.ContainsKey(name))
        {
            soundEffects.Add(name, clip);
        }
    }
    
    // Music controls
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource != null && clip != null)
        {
            if (musicSource.clip == clip && musicSource.isPlaying)
                return;
            
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }
    
    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }
    
    public void PlayARMusic()
    {
        PlayMusic(arSceneMusic);
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }
    
    public void ResumeMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }
    
    // SFX controls
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void PlaySFX(string soundName)
    {
        if (soundEffects.ContainsKey(soundName))
        {
            PlaySFX(soundEffects[soundName]);
        }
        else
        {
            Debug.LogWarning($"Sound effect '{soundName}' not found!");
        }
    }
    
    public void PlayButtonClick()
    {
        PlaySFX("ButtonClick");
    }
    
    public void PlayAnimalPlaced()
    {
        PlaySFX("AnimalPlaced");
    }
    
    public void PlayAnimalSelected()
    {
        PlaySFX("AnimalSelected");
    }
    
    // Volume controls
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
        
        // Save to PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
        
        // Save to PlayerPrefs
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    
    public void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        }
        
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }
    }
    
    // Mute controls
    public void MuteMusic(bool mute)
    {
        if (musicSource != null)
        {
            musicSource.mute = mute;
        }
    }
    
    public void MuteSFX(bool mute)
    {
        if (sfxSource != null)
        {
            sfxSource.mute = mute;
        }
    }
    
    public void MuteAll(bool mute)
    {
        MuteMusic(mute);
        MuteSFX(mute);
    }
}
