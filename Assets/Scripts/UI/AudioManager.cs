using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer EffectAudioMixer;
    public AudioMixer MusicAudioMixer;

    public AudioSource carStartSource;
    public AudioSource carDrivingSource;
    public AudioSource coinSoundSource;
    public AudioSource musicSource;

    public AudioClip carStartClip;
    public AudioClip carDrivingClip;
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


    public void PlayCarStartSound()
    {
        carStartSource.clip = carStartClip;
        carStartSource.Play();
    }

    public void PlayCarDrivingSound()
    {
        carDrivingSource.clip = carDrivingClip;
        carDrivingSource.Play();
    }

    public void StopCarDrivingSound()
    {
        carDrivingSource.clip = carDrivingClip;
        carDrivingSource.Stop();
    }

    public void StopCarStartSound()
    {
        carStartSource.clip = carStartClip;
        carStartSource.Stop();
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
    isMuted = !isMuted; // Odwracamy bieżący stan wyciszenia

    if (isMuted)
    {
        // Jeśli jest wyciszone, zatrzymujemy wszystkie źródła dźwięku i ustawiamy głośność na minimalną
        EffectAudioMixer.SetFloat("EffectsVolume", -80f); // Minimalna głośność
        MusicAudioMixer.SetFloat("MusicVolume", -80f); // Minimalna głośność
    }
    else
    {   
        
        // Jeśli nie jest wyciszone, przywracamy zapisane głośności
        SetEffectsVolume(effectsVolume);
        SetMusicVolume(musicVolume);
    }

    return isMuted; // Zwracamy bieżący stan wyciszenia
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