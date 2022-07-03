using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float selfGacha;

    private void Start()
    {
        selfGacha = Random.Range(.3f, .5f) + (GameManager.Instance.multiplier * 0.0005f);

    }
    // Enemy movement
    private void Update()
    {
        transform.Translate(0, GameManager.Instance.enemySpd * selfGacha * Time.deltaTime, 0); // add delta time so enmmy can stop moving when game paused
        //Debug.Log(Time.deltaTime); //0.0005f
    }

    // If being shooted
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        // If exeeds the world borders, then destroy
        if (trigger.gameObject.CompareTag("World Border"))
        {
            Destroy(this.gameObject);
            return;
        }

        // game over
        if (trigger.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over");
            MenuManager.Instance.gameOverPanel.SetActive(true);
            Time.timeScale = 0;
            MenuManager.Instance.state = MenuManager.State.Menu;
        }

    }

}
