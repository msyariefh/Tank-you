using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    private int enemies;
    private float cooldown;
    private int waveNum = 0;

    [SerializeField]
    private GameObject m_EnemyPrefab;

    private IObjectPool<GameObject> m_EnemyPool;
    public IObjectPool<GameObject> EnemyPool
    {
        get
        {
            m_EnemyPool ??= new ObjectPool<GameObject>(
                    // On Create Pool
                    () =>
                    {
                        var go = Instantiate(m_EnemyPrefab);

                        // pool
                        var itemPool = go.GetComponent<IPoolableItem>();
                        itemPool.Pool = EnemyPool;
                        return go;

                    },
                    // On Get Pool
                    (GameObject go) =>
                    {
                        CalculatePositionAndRotation(go.transform);
                        go.SetActive(true);
                    },
                    // On Return Pool
                    (GameObject go) =>
                    {
                        go.SetActive(false);
                    },
                    // On Pool Destroy
                    (GameObject go) =>
                    {
                        Destroy(go);
                    },
                    // Default Capacity
                    defaultCapacity: 20
                    );
            return m_EnemyPool;
        }
    }
    private void CalculatePositionAndRotation(Transform t)
    {
        var pos = Vector2.zero;
        var tankPos = GameManager.Instance.Tank.position;

        pos.x = Random.Range(-4.0f, 4.0f);
        pos.y = Random.Range(6.0f, 6.0f + enemies);
        var attackAngle = Mathf.Atan2(
            pos.y - tankPos.y, pos.x - (tankPos.x + 
            Random.Range(1.2f, -1.2f))) * Mathf.Rad2Deg;
        t.position = pos;
        t.localRotation = Quaternion.Euler(new Vector3(0, 0, attackAngle + 90));
    }
    private void OnDestroy()
    {
        EnemyPool.Clear();
    }

    private void Start()
    {
        //InvokeRepeating(string(SpawnEnemies), 1, GameManager.Instance.spawnerCD); 
        enemies = GameManager.Instance.InitialSpawnNumber;
        cooldown = GameManager.Instance.spawnerCD;
    }
    private void Update()
    {
        if (!m_stillSpawning)
        {
            var newCD = cooldown + (.2f * (enemies / 5));
            StartCoroutine(SpawnEnemiesAfter(enemies, newCD));
            cooldown += GameManager.Instance.SpawnerTimeGrowth * waveNum;
        }
    }

    bool m_stillSpawning = false;
    IEnumerator SpawnEnemiesAfter(int num, float time)
    {
        m_stillSpawning = true;
        waveNum++;
        var newTime = time / enemies;
        for (int i = num; i >0; i--)
        {
            EnemyPool.Get();
            yield return new WaitForSeconds(newTime);
        }

        enemies++;
        if (enemies % 4 == 0) GameManager.Instance.Multiplier++;
        m_stillSpawning = false;
    }
    private void SpawnEnemies()
    {
        Vector3 pos = transform.position;
        Vector3 tankPos = GameManager.Instance.Tank.position;

        for (int i = 0; i < enemies; i++)
        {
            pos.x = Random.Range(-4.0f, 4.0f);
            pos.y = Random.Range(6.0f, 6 + 1.5f * enemies);
            float angle = Mathf.Atan2(pos.y - tankPos.y, pos.x - (tankPos.x + Random.Range(-1.2f, 1.2f))) * Mathf.Rad2Deg;

            Instantiate(m_EnemyPrefab, pos, Quaternion.Euler(new Vector3(0, 0, angle + 90)));
        }
        if (enemies >= 4)
        {
            if (GameManager.Instance.Multiplier >= 20) { return; }
            GameManager.Instance.Multiplier++;
            return;
        }

        enemies += GameManager.Instance.InitialSpawnNumber;

    }

}
