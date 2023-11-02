using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject vehiclePanel;
    public GameObject scorePanel;
    public Slider effectsVolumeSlider;
    public Slider musicVolumeSlider;
    public Toggle lowQualityToggle;
    public Toggle mediumQualityToggle;
    public Toggle highQualityToggle;
    public GameObject OpenOptions;
    public GameObject CloseOptions;
    public GameObject MuteButton;
    public GameObject UnmuteButton;
    public static bool isOptionsPanelActive = false;

    private float originalVolume;
    float currentVolume;

    void Start()
    {
        musicVolumeSlider.value = AudioManager.instance.GetMusicVolume();
        effectsVolumeSlider.value = AudioManager.instance.GetEffectsVolume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Możesz dostosować klawisz do otwierania opcji.
        {
            ToggleOptions();
        }
    }

    public void ToggleOptions()
    {
        if(GameManager.instance.gameStarted == false && scorePanel.activeSelf)
        {
            scorePanel.SetActive(false);
        }
        PauseGame();

        optionsPanel.SetActive(true);

        CloseOptions.SetActive(true);

        OpenOptions.SetActive(false);

        if(vehiclePanel.activeSelf == true)
        {
            vehiclePanel.SetActive(false);
        }

        isOptionsPanelActive = true;

        OnOptionsMenuOpened(); ///////
    }

    public void UntoggleOptions()
    {   
        if(GameManager.instance.gameStarted == false && !scorePanel.activeSelf)
        {
            scorePanel.SetActive(true);
        }

        ResumeGame();

        optionsPanel.SetActive(false);

        CloseOptions.SetActive(false);

        OpenOptions.SetActive(true);

        if(vehiclePanel.activeSelf == false && GameManager.instance.gameStarted == false)
        {
            vehiclePanel.SetActive(true);
        }

        isOptionsPanelActive = false;
    }

    public void SetMusicVolume()
    {
        float volume = musicVolumeSlider.value;
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
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    void OnOptionsMenuOpened()
    {
    musicVolumeSlider.value = AudioManager.instance.GetMusicVolume();
    effectsVolumeSlider.value = AudioManager.instance.GetEffectsVolume();
    }

     public void OnVolumeSliderChangedEffects()
    {
        float volumeEffects = effectsVolumeSlider.value;
        AudioManager.instance.SetEffectsVolume(volumeEffects);
    }

    public void OnVolumeSliderChangedMusic()
    {
        float volumeMusic = musicVolumeSlider.value;
        AudioManager.instance.SetMusicVolume(volumeMusic);
    }

}
