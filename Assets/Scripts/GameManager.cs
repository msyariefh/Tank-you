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

        //DontDestroyOnLoad(gameObject);
    }

    /** 
     * =================
     * Global Variables
     * =================
     */

    // Game Stats
    
    [Header("TANK STATS")]
    [Tooltip("Tank's shoots per second")]
    public float TankShootPerSecond = .8f; // Shoots per second
    [Tooltip("Tank's weapon velocity when shot")]
    public float TankWeaponSpeed = .1f; // Speeds of a shoot
    
    [HideInInspector]public float sensitivity = 1.0f; // Move sensitivity
    [HideInInspector][Range(0f, 1f)] public float maximumShootAngle = 0.35f; // Rotation limitation (in z rotation)

    // Enemy Stats
    [Header("ENEMY STATS")]
    [Tooltip("Initial Enemy(es) number per spawn")]
    public int InitialSpawnNumber = 2;  // Enemies per one spawn
    [Tooltip("Enemy speed when attacking")]
    public float EnemySpeed = .05f; // Enemy speed [scaled once more in instantiate]
    [Tooltip("Time before next enemy bulk spawn")]
    public float spawnerCD = 2.0f; // Cooldown for enemy spawner
    [Tooltip("Time growth after some spawns")]
    public float SpawnerTimeGrowth = 0f;

    [Header("MISC")]

    public GameObject scoreboard; // Scoreboard
    [HideInInspector] public int Multiplier = 0;

    // Player (Tank) Stats
    [HideInInspector]public int score = 0; // Increase when player destroy enemy
    public Transform Tank;
    public bool activateHelper = false;
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
    public string AddZeros(int score)
    {
        // pattern 0000
        if (score < 10) { return "000" + score; }
        if (score < 100) { return "00" + score; }
        if (score < 1000) { return "0" + score; }
        if (score < 10000) { return score.ToString(); }

        return "0000";
    }
    AudioManager ManagerAudio;
    public void PlaySoundFX(string name)
    {
        ManagerAudio ??= FindFirstObjectByType<AudioManager>();
        ManagerAudio.PlaySound(name);
    }
    
}
