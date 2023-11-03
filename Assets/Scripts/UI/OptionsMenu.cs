using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject vehiclePanel;
    public GameObject scorePanel;
    public Toggle lowQualityToggle;
    public Toggle mediumQualityToggle;
    public Toggle highQualityToggle;
    public GameObject OpenOptions;
    public GameObject CloseOptions;
    public GameObject MuteButton;
    public GameObject UnmuteButton;
    public static bool isOptionsPanelActive = false;

    public Slider effectsVolumeSlider; // Skorygowano przypisanie sliderów z poziomu inspektora
    public Slider musicVolumeSlider; 

    void Awake()
    {
        // Przypisz slidery w inspektorze lub programowo, jeśli to konieczne.
        effectsVolumeSlider = FindObjectOfType<Slider>(); 
        musicVolumeSlider = FindObjectOfType<Slider>();
    }

    void Start()
    {
        // Pobierz zapisane ustawienia sliderów z PlayerPrefs i zastosuj je
        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            float effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");
            AudioManager.instance.SetEffectsVolume(effectsVolume);
            effectsVolumeSlider.value = effectsVolume;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            AudioManager.instance.SetMusicVolume(musicVolume);
            musicVolumeSlider.value = musicVolume;
        }
    }


    public void ToggleOptions()
    {
        if (GameManager.instance.gameStarted == false && scorePanel.activeSelf)
        {
            scorePanel.SetActive(false);
        }
        PauseGame();

        optionsPanel.SetActive(true);

        CloseOptions.SetActive(true);

        OpenOptions.SetActive(false);

        if (vehiclePanel.activeSelf == true)
        {
            vehiclePanel.SetActive(false);
        }

        isOptionsPanelActive = true;

        OnOptionsMenuOpened();
    }

    public void UntoggleOptions()
    {
        if (GameManager.instance.gameStarted == false && !scorePanel.activeSelf)
        {
            scorePanel.SetActive(true);
        }

        ResumeGame();

        optionsPanel.SetActive(false);

        CloseOptions.SetActive(false);

        OpenOptions.SetActive(true);

        if (vehiclePanel.activeSelf == false && GameManager.instance.gameStarted == false)
        {
            vehiclePanel.SetActive(true);
        }

        isOptionsPanelActive = false;
    }

    public void SetEffectVolume() // Skorygowano nazwę funkcji od ustawiania głośności efektów
    {
        float volume = effectsVolumeSlider.value; // Poprawiono odczyt wartości z odpowiedniego slidera
        AudioManager.instance.SetEffectsVolume(volume); // Ustawiono głośność efektów
        AudioManager.instance.SaveAudioSettings(); // Zapisano ustawienia
    }

    public void SetMusicVolume() // Skorygowano nazwę funkcji od ustawiania głośności muzyki
    {
        float volume = musicVolumeSlider.value; // Poprawiono odczyt wartości z odpowiedniego slidera
        AudioManager.instance.SetMusicVolume(volume); // Ustawiono głośność muzyki
        AudioManager.instance.SaveAudioSettings(); // Zapisano ustawienia
    }


    public void SetGraphicsQuality()
    {
        if (lowQualityToggle.isOn)
        {
            QualitySettings.SetQualityLevel(0);
        }
        else if (mediumQualityToggle.isOn)
        {
            QualitySettings.SetQualityLevel(2);
        }
        else if (highQualityToggle.isOn)
        {
            QualitySettings.SetQualityLevel(4);
        }
    }

    public void ToggleMute()
    {
        UnmuteButton.SetActive(true);
        MuteButton.SetActive(false);
        bool isMuted = AudioManager.instance.ToggleMute();
    }

    public void UntoggleMute()
    {
        UnmuteButton.SetActive(false);
        MuteButton.SetActive(true);
        bool isMuted = AudioManager.instance.ToggleMute();
    }

    private void PauseGame()
    {
        AudioManager.instance.StopCarSound();
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        AudioManager.instance.PlayCarSound();
        Time.timeScale = 1;
    }

    void OnOptionsMenuOpened()
    {
        musicVolumeSlider.value = AudioManager.instance.GetMusicVolume();
        effectsVolumeSlider.value = AudioManager.instance.GetEffectsVolume();
    }
}
