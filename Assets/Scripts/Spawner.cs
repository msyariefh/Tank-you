using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int enemies;

    [SerializeField]
    private GameObject enemyPrefab;

    private void Start()
    {
        InvokeRepeating("Serang", 1, GameManager.Instance.spawnerCD);
        enemies = GameManager.Instance.initialEnemies;
    }

    private void Serang()
    {
        Vector3 pos = transform.position;
        Vector3 tankPos = GameManager.Instance.tankPosition.position;

        for (int i = 0; i < enemies; i++)
        {
            pos.x = Random.Range(-4.0f, 4.0f);
            pos.y = Random.Range(6.0f, 6 + 1.5f * enemies);
            float angle = Mathf.Atan2(pos.y - tankPos.y, pos.x - (tankPos.x + Random.Range(-1.2f, 1.2f))) * Mathf.Rad2Deg;

            Instantiate(enemyPrefab, pos, Quaternion.Euler(new Vector3(0, 0, angle + 90)));
        }
        if (enemies >= 6)
        {
            if (GameManager.Instance.multiplier >= 10) { return; }
            GameManager.Instance.multiplier++;
            return;
        }

        enemies += GameManager.Instance.initialEnemies;

    }

}
