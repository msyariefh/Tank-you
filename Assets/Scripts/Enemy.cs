using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Vector3 target;
    private float selfGacha;

    private void Start()
    {
        //target = new Vector3(Random.Range(-1.0f, 1.0f), -6, 1);
        target = new Vector3(Random.Range(-1.0f, 1.0f), GameManager.Instance.tankPosition.transform.position.y, 1);
        selfGacha = Random.Range(.1f, .5f);

    }
    // Enemy movement
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, selfGacha * 
            GameManager.Instance.enemySpd);
    }

    // If being shooted
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        // If exeeds the world borders, then destroy
        if (trigger.gameObject.CompareTag("World Border"))
        {
            Destroy(this.gameObject);
        }
    }

}