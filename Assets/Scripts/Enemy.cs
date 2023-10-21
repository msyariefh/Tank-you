using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour, IPoolableItem
{
    //private float selfGacha;

    public IObjectPool<GameObject> Pool { get ; set; }

    private void OnDisable()
    {
        Pool.Release(gameObject);
    }
    private float additionalSpeed;
    private void OnEnable()
    {
        additionalSpeed = Random.Range(.3f, .5f) + GameManager.Instance.Multiplier * .005f;
    }

    // Enemy movement
    private void Update()
    {
        //transform.Translate(0, GameManager.Instance.EnemySpeed * selfGacha * Time.deltaTime, 0); // add delta time so enmmy can stop moving when game paused
        //Debug.Log(Time.deltaTime); //0.0005f

        transform.Translate(0, (1f + additionalSpeed) *
           GameManager.Instance.EnemySpeed *
           Time.deltaTime * 30f, 0);
    }

    // If being shooted
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        // If exeeds the world borders, then destroy
        if (trigger.gameObject.CompareTag("World Border"))
        {
            gameObject.SetActive(false);
            return;
        }

        // game over
        if (trigger.gameObject.CompareTag("Player"))
        {
            MenuManager.Instance.GameOver();
        }

    }

}
