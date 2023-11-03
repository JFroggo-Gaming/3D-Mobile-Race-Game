using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer EffectAudioMixer;
    public AudioMixer MusicAudioMixer;

    public AudioSource carSoundSource;
    public AudioSource coinSoundSource;
    public AudioSource musicSource;

    public AudioClip carSoundClip;
    public AudioClip coinSoundClip;
    public AudioClip musicClip;
    public AudioClip gameOverSoundClip;

    private float effectsVolume = 1.0f; // Domyślna głośność efektów
    private float musicVolume = 1.0f; // Domyślna głośność muzyki

    private bool isMuted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Pobranie zapisanej głośności z PlayerPrefs
        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }

        // Zastosowanie głośności
        SetEffectsVolume(effectsVolume);
        SetMusicVolume(musicVolume);
    }


    public void PlayCarSound()
    {
        carSoundSource.clip = carSoundClip;
        carSoundSource.Play();
    }

    public void StopCarSound()
    {
        carSoundSource.clip = carSoundClip;
        carSoundSource.Stop();
    }

    public void PlayGameOverSound()
    {
        coinSoundSource.clip = gameOverSoundClip;
        coinSoundSource.Play();
    }

    public void PlayCoinSound()
    {
        coinSoundSource.clip = coinSoundClip;
        coinSoundSource.Play();
    }

    public bool ToggleMute()
    {
        isMuted = !isMuted;

        // Zmiana głośności efektów i muzyki w zależności od flagi wyciszenia
        if (isMuted)
        {
            SetEffectsVolume(0f);
            SetMusicVolume(0f);
        }
        else
        {
            SetEffectsVolume(effectsVolume);
            SetMusicVolume(musicVolume);
        }

        return isMuted;
    }

    public float GetEffectsVolume()
    {
        return effectsVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetEffectsVolume(float volumeEffects)
    {
        effectsVolume = volumeEffects;
        if (!isMuted)
        {
            EffectAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(volumeEffects) * 20); // Poprawiono klucz AudioMixer'a
        }
    }

    public void SetMusicVolume(float volumeMusic)
    {
        musicVolume = volumeMusic;
        if (!isMuted)
        {
            MusicAudioMixer.SetFloat("MusicVolume", Mathf.Log10(volumeMusic) * 20);
        }
    }

    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }
}