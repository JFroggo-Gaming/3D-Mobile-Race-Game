// OptionsMenu.cs

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu instance;
    public GameObject optionsPanel;
    public GameObject vehiclePanel;
    public GameObject scorePanel;

    public GameObject scoreListPanel;
    public TMP_Text Score1;
    public TMP_Text Score2;
    public TMP_Text Score3;
    public TMP_Text Score4;
    public TMP_Text Score5;

    public Toggle lowQualityToggle;
    public Toggle mediumQualityToggle;
    public Toggle highQualityToggle;
    public GameObject OpenOptions;
    public GameObject CloseOptions;
    public GameObject MuteButton;
    public GameObject UnmuteButton;

    public static bool isOptionsPanelActive = false;

    private bool isOpen; // open the list with best scores
    public Slider effectsVolumeSlider; // Slider dla efektów
    public Slider musicVolumeSlider; // Slider dla muzyki

    void Start()
    {
        // Przypisanie wartości z PlayerPrefs do odpowiednich sliderów
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

        // Inicjalizacja listy najlepszych wyników
        InitHighScores();
    }

    void InitHighScores()
{
    // Odczytaj najlepsze wyniki z PlayerPrefs i zaktualizuj UI
    for (int i = 0; i < 5; i++)
    {
        if (PlayerPrefs.HasKey("HighScore" + (i + 1)))
        {
            int highScore = PlayerPrefs.GetInt("HighScore" + (i + 1));
            SetHighScoreText(i, highScore);
        }
        else
        {
            int defaultHighScore = 0;
            SetHighScoreText(i, defaultHighScore);
        }
    }
}


    public bool ToggleScoreList()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            scoreListPanel.SetActive(true);

            if (vehiclePanel.activeSelf == true)
            {
                vehiclePanel.SetActive(false);
            }

            if (optionsPanel.activeSelf == true)
            {
                optionsPanel.SetActive(false);
                CloseOptions.SetActive(false);
                OpenOptions.SetActive(true);
                ResumeGame();
                isOptionsPanelActive = false;
                GameManager.instance.ResumeScoreIncrease();
            }
        }
        else
        {
            if (scoreListPanel.activeSelf)
            {
                scoreListPanel.SetActive(false);
            }

            if (vehiclePanel.activeSelf == false && GameManager.instance.gameStarted == false)
            {
                vehiclePanel.SetActive(true);
            }
        }
        return isOpen;
    }

    public void ToggleScoreListFunction()
    {
        ToggleScoreList();
        
    }

    public void ToggleOptions()
    {
        if (GameManager.instance.gameStarted == false && scorePanel.activeSelf)
        {
            scorePanel.SetActive(false);
        }
        PauseGame();

        GameManager.instance.StopScoreIncrease();

        optionsPanel.SetActive(true);
        CloseOptions.SetActive(true);
        OpenOptions.SetActive(false);

        if (vehiclePanel.activeSelf == true)
        {
            vehiclePanel.SetActive(false);
        }

        isOptionsPanelActive = true;

        OnOptionsMenuOpened();

        if (scoreListPanel.activeSelf)
        {
            scoreListPanel.SetActive(false);
        }

        isOpen = false;
    }

    public void UntoggleOptions()
    {
        if (GameManager.instance.gameStarted == false && !scorePanel.activeSelf && GameManager.instance.score > 0)
        {
            scorePanel.SetActive(true);
            optionsPanel.SetActive(false);
            CloseOptions.SetActive(false);
            OpenOptions.SetActive(true);
            ResumeGame();
            isOptionsPanelActive = false;
            return;
        }

        ResumeGame();

        GameManager.instance.ResumeScoreIncrease();

        optionsPanel.SetActive(false);
        CloseOptions.SetActive(false);
        OpenOptions.SetActive(true);

        if (vehiclePanel.activeSelf == false && GameManager.instance.gameStarted == false)
        {
            vehiclePanel.SetActive(true);
        }

        isOptionsPanelActive = false;
        isOpen = true;
    }

    public void SetEffectVolume()
    {
        float volume = effectsVolumeSlider.value;
        AudioManager.instance.SetEffectsVolume(volume);
        AudioManager.instance.SaveAudioSettings();
    }

    public void SetMusicVolume()
    {
        float volume = musicVolumeSlider.value;
        AudioManager.instance.SetMusicVolume(volume);
        AudioManager.instance.SaveAudioSettings();
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
        AudioManager.instance.StopCarDrivingSound();
        AudioManager.instance.StopCarStartSound();
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        if (AudioManager.instance.carDrivingSource.isPlaying)
        {
            AudioManager.instance.PlayCarDrivingSound();
        }
        if (AudioManager.instance.carStartSource.isPlaying)
        {
            AudioManager.instance.PlayCarStartSound();
        }
        Time.timeScale = 1;
        Debug.Log("RESUME");
    }

    void OnOptionsMenuOpened()
    {
        musicVolumeSlider.value = AudioManager.instance.GetMusicVolume();
        effectsVolumeSlider.value = AudioManager.instance.GetEffectsVolume();
    }

    // Metoda do aktualizacji najlepszych wyników w UI
    public void SetHighScoreText(int index, int score)
    {
        switch (index)
        {
            case 0:
                Score1.text = score.ToString();
                break;
            case 1:
                Score2.text = score.ToString();
                break;
            case 2:
                Score3.text = score.ToString();
                break;
            case 3:
                Score4.text = score.ToString();
                break;
            case 4:
                Score5.text = score.ToString();
                break;
        }
    }
}
