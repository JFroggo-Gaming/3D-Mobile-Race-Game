using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text Points; // TextMeshPro komponent

    public GameObject StartPlatforms;
    public GameObject ScorePanel;
    public GameObject PlayConfirmPanel; // The Panel informing a player that he has to tap on the screen to start the game
    public bool gameStarted;
    public int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        // Zwiększaj punktację o 1 punkt na sekundę
        IncreaseScore(1);
    }

    UpdateScoreText();
}


    public void GameStart()
    {   
        PlayConfirmPanel.SetActive(false);
        gameStarted = true;
        VehicleManager.instance.PlatformSpawner.SetActive(true);
        score = 0;
        UpdateScoreText();
        StartCoroutine(IncreaseScoreOverTime());
    }
    
    public void GameOver()
    {
        gameStarted = false;
        VehicleManager.instance.PlatformSpawner.SetActive(false);
        ScorePanel.SetActive(true);
        StartPlatforms.SetActive(false);
        StopAllCoroutines(); // Przerwij wszystkie korutyny
        // Ustaw prędkość samochodu na 0, aby go zatrzymać
        VehicleManager.instance.currentVehicle.GetComponent<CarController>().MoveSpeed = 0;
    }


    public void ReloadLevel()
    {      
        SceneManager.LoadScene("Game");
        ScorePanel.SetActive(false);
        StartPlatforms.SetActive(true);
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
