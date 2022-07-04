using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public TMP_Text score;
    public TMP_Text HScore;
    public GameObject newBadge;
    public enum State
    {
        Menu,
        Gameplay
    }
    public State state;


    public bool isPause;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance == this) { Destroy(this); }

        //DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape) && state == State.Menu)
            {
                // Quit the application
                Application.Quit();
            }
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPause = false;
        state = State.Gameplay;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        isPause = true;
        state = State.Menu;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        state = State.Gameplay;
    }
    public void MenuGame()
    {
        SceneManager.LoadScene(0);
        state = State.Menu;
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        state = State.Gameplay;
        gameOverPanel.SetActive(false);
    }
    public void GameOver()
    {
        
        int currentScore = GameManager.Instance.score;
        if (PlayerPrefs.HasKey("HighScore")) 
        { 
            PlayerPrefs.SetInt("HighScore", currentScore);
            if (currentScore > 0) { newBadge.SetActive(true); }
        }
        else if (PlayerPrefs.GetInt("HighScore") < currentScore) 
        { 
            PlayerPrefs.SetInt("HighScore", currentScore);
            newBadge.SetActive(true);
        }
        else
        {
            newBadge.SetActive(false);
        }
        int highScore = PlayerPrefs.GetInt("HighScore");
        score.text = GameManager.Instance.AddZeros(currentScore);
        HScore.text = GameManager.Instance.AddZeros(highScore);
        gameOverPanel.SetActive(true);
        state = State.Menu;
        Time.timeScale = 0f;
    }
}

