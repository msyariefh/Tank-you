using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public TMP_Text score;
    public TMP_Text HScore;
    public GameObject newBadge;
    private AdsManager interstitialAds;
    private AudioManager audioManager;

    //private AdsManager AdsManager => interstitialAds ??= FindFirstObjectByType<AdsManager>();
    private AudioManager Audio=> audioManager ??= FindFirstObjectByType<AudioManager>();

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
        Audio.RemoveEffectOnSound();
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
        Audio.AddEffectOnSound();
    }
    public void StartGame()
    {
        Audio.RemoveEffectOnSound();
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        state = State.Gameplay;
    }
    public void MenuGame()
    {
        //FindObjectOfType<InterstitialAds>().LoadAd();
        if (IsTimeToLoadAds()) AdsManager.Instance.LoadAd();
        Audio.RemoveEffectOnSound();
        StartCoroutine(ExecuteAfterAds(() =>
        {
            SceneManager.LoadScene(0);
            state = State.Menu;
            Time.timeScale = 1f;
            Audio.PlayBgmSound("MainMenu");
        }));
        //FindObjectOfType<AudioManager>().RemoveEffectOnSound("Arena");
        //SceneManager.LoadScene(0);
        //state = State.Menu;
        //Time.timeScale = 1f;
        //FindObjectOfType<AudioManager>().PlaySound("Arena", "MainMenu");
    }
    public void RetryGame()
    {
        if (IsTimeToLoadAds()) AdsManager.Instance.LoadAd();
        StartCoroutine(ExecuteAfterAds(() =>
        {
            Audio.PlayBgmSound("Arena");
            Audio.RemoveEffectOnSound();
            SceneManager.LoadScene(1);
            Time.timeScale = 1f;
            state = State.Gameplay;
            gameOverPanel.SetActive(false); ;
        }));
        
    }
    public void GameOver()
    {
        if (IsTimeToLoadAds()) AdsManager.Instance.LoadAd();
        Audio.AddEffectOnSound();
        int currentScore = GameManager.Instance.score;
        if (!PlayerPrefs.HasKey("HighScore"))
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
        PlayerPrefs.Save();
        int highScore = PlayerPrefs.GetInt("HighScore");
        score.text = GameManager.Instance.AddZeros(currentScore);
        HScore.text = GameManager.Instance.AddZeros(highScore);
        gameOverPanel.SetActive(true);
        state = State.Menu;
        
        Time.timeScale = 0f;
    }

    IEnumerator ExecuteAfterAds(Action afterAds)
    {
        yield return AdsManager.Instance.WaitingForAds();
        afterAds?.Invoke();
    }
    
    float lastTimeLoadAds = 0f;
    private int gameplaycount = 0;
    private bool IsTimeToLoadAds()
    {
        var cTime = Time.unscaledTime;
        var deltaTime = cTime - lastTimeLoadAds;
        Debug.Log($"[HiDE] {cTime} | {deltaTime} | {lastTimeLoadAds} | {gameplaycount} ");
        gameplaycount++;
        // Load ads if played more than 3mins
        if (deltaTime > 240)
        {
            gameplaycount = 0;
            lastTimeLoadAds = cTime;
            return true;
        }
        // 2 tries within 60 sec
        if (gameplaycount >=2 && deltaTime > 30)
        {
            gameplaycount = 0;
            lastTimeLoadAds = cTime;
            return true;
        }

        return false;
    }
}

