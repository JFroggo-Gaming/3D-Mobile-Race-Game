using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer EffectAudioMixer; // Referencja do Audio Mixer
    public AudioMixer MusicAudioMixer;

    public AudioSource carSoundSource; // Źródło dźwięku samochodu
    public AudioSource coinSoundSource; // Źródło dźwięków gry
    public AudioSource musicSource;

    public AudioClip carSoundClip; // Dźwięk samochodu
    public AudioClip coinSoundClip; // Dźwięk monet
    public AudioClip musicClip;
    public AudioClip gameOverSoundClip; // Dźwięk Game Over

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void PlayCarSound()
    {
        carSoundSource.clip = carSoundClip;
        carSoundSource.Play();
        Debug.Log("Car Sound");
    }

    public void StopCarSound()
    {
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
        bool isMuted = !carSoundSource.mute && !coinSoundSource.mute; // Invert the current mute state.
        coinSoundSource.mute = isMuted;
        carSoundSource.mute = isMuted;
        return isMuted;
    }

    public float GetEffectsVolume()
    {
        float valueEffects;
        EffectAudioMixer.GetFloat("MasterVolume", out valueEffects);
        return valueEffects;
    }

    public float GetMusicVolume()
    {
        float valueMusic;
        MusicAudioMixer.GetFloat("MusicVolume", out valueMusic);
        return valueMusic;
    }

    public void SetEffectsVolume(float volumeEffects)
    {
        EffectAudioMixer.SetFloat("MasterVolume", Mathf.Log10(volumeEffects) * 20);
    }

    public void SetMusicVolume(float volumeMusic)
    {
        MusicAudioMixer.SetFloat("MusicVolume", Mathf.Log10(volumeMusic) * 20);
    }
}