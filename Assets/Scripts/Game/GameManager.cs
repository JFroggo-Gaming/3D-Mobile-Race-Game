// GameManager.cs

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text Points; // TextMeshPro komponent

    public GameObject StartPlatforms;
    public GameObject ScorePanel;
    public GameObject PlayConfirmPanel; // The Panel informing a player that he has to tap on the screen to start the game
    public bool gameStarted;
    public Button scoreListButton;
    public int score = 0;
    private bool canIncreaseScore = true; // Dodana zmienna do kontroli zwiększania punktacji

    // Zmienna do przechowywania najlepszych wyników
    private int[] highScores = new int[5];

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // Odczytaj najlepsze wyniki z PlayerPrefs
        for (int i = 0; i < highScores.Length; i++)
        {
            if (PlayerPrefs.HasKey("HighScore" + (i + 1)))
            {
                highScores[i] = PlayerPrefs.GetInt("HighScore" + (i + 1));
            }
            else
            {
                highScores[i] = 0; // Domyślna wartość, jeśli wynik nie jest jeszcze zapisany
            }
        }
    }

    void Update()
    {
        if (!gameStarted && PlayConfirmPanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
        }

        if (gameStarted)
        {
            if (canIncreaseScore)
            {
                IncreaseScore(1);
            }
        }

        UpdateScoreText();
    }

    public void StopScoreIncrease()
    {
        canIncreaseScore = false;
    }

    public void ResumeScoreIncrease()
    {
        canIncreaseScore = true;
    }

    public void GameStart()
    {
        PlayConfirmPanel.SetActive(false);
        gameStarted = true;
        VehicleManager.instance.PlatformSpawner.SetActive(true);
        score = 0;
        UpdateScoreText();
        StartCoroutine(IncreaseScoreOverTime());
        AudioManager.instance.PlayCarStartSound();
        AudioManager.instance.Invoke("PlayCarDrivingSound", 8f);

        // Wyłącz przycisk z OptionsMenu
        scoreListButton.interactable = false;
    }

    public void GameOver()
    {
        gameStarted = false;
        VehicleManager.instance.PlatformSpawner.SetActive(false);
        ScorePanel.SetActive(true);
        StartPlatforms.SetActive(false);
        StopAllCoroutines(); // Przerwij wszystkie korutyny

        // Zatrzymaj dźwięk samochodu
        AudioManager.instance.StopCarDrivingSound();
        AudioManager.instance.StopCarStartSound();
        // Odtwórz dźwięk Game Over
        AudioManager.instance.PlayGameOverSound();

        // Sprawdź, czy uzyskano nowy najlepszy wynik i zapisz go
        for (int i = 0; i < highScores.Length; i++)
        {
            if (score > highScores[i])
            {
                // Przesuń inne wyniki w dół
                for (int j = highScores.Length - 1; j > i; j--)
                {
                    highScores[j] = highScores[j - 1];
                }
                highScores[i] = score;
                // Zapisz najlepsze wyniki do PlayerPrefs
                for (int j = 0; j < highScores.Length; j++)
                {
                    PlayerPrefs.SetInt("HighScore" + (j + 1), highScores[j]);
                }
                break;
            }
        }
    }

    public void ReloadLevel()
    {
        scoreListButton.interactable = true;
        SceneManager.LoadScene("Game");
    }

    public void ReloadLevelAfterSelect()
    {
        PlayConfirmPanel.SetActive(true);
        ScorePanel.SetActive(false);
        StartPlatforms.SetActive(true);
    }

    void UpdateScoreText()
    {
        Points.text = score.ToString();
    }

    void IncreaseScore(int value)
    {
        score += value;
        UpdateScoreText();
    }

    IEnumerator IncreaseScoreOverTime()
    {
        while (true)
        {
            IncreaseScore(1);
            yield return new WaitForSeconds(1f);
        }
    }
}
