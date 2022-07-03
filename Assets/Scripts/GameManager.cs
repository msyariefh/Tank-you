using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
  // Instance
  public static GameManager Instance { get; private set; }
    // Init
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance == this) { Destroy(this); }

        DontDestroyOnLoad(gameObject);
    }

    /** 
     * =================
     * Global Variables
     * =================
     */

    // Game Stats
    public int initialEnemies = 2;  // Enemies per one spawn
    public GameObject scoreboard; // Scoreboard
    public int multiplier = 0;

    // Player (Tank) Stats
    public int score = 0; // Increase when player destroy enemy
    public float shootSpeed = .8f; // Shoots per second
    public float shootVelocity = .1f; // Speeds of a shoot
    public float sensitivity = 1.0f; // Move sensitivity
    public float maximumShootAngle = 0.35f; // Rotation limitation (in z rotation)
    public Transform tankPosition;
    public bool activateHelper = false;

    // Enemy Stats
    public float enemySpd = .05f; // Enemy speed [scaled once more in instantiate]
    public float spawnerCD = 2.0f; // Cooldown for enemy spawner


    public MenuManager menu;
    
    /**
     * ================
     * Global Functions
     * ================
     */

    // Add Score to user stats and refresh scoreboard
    public void AddScores(int scoreToAdd)
    {
        score += scoreToAdd; // Add (n) to user score
        TMP_Text txt = scoreboard.GetComponent<TMP_Text>();
        txt.text = AddZeros(); // Convert based on template before change scoreboard
    }

    public string AddZeros()
    {
        // pattern 0000
        if (score < 10) { return "000" + score; }
        if (score < 100) { return "00" + score; }
        if (score < 1000) { return "0" + score; }
        if (score < 10000) { return score.ToString(); }

        return "0000";
    }

    public void GameOver()
    {
        // Game over
        Debug.Log("GAME OVER");
        Application.Quit();
    }

}
