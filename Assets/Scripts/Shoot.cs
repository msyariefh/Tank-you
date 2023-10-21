using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shoot : MonoBehaviour
{
    private float shootSpeed;
    private float multiplier;
    private bool isAttacking = false;

    [SerializeField] private GameObject m_BulletPrefab;
    public Animator riffleAnimation;
    public Animator bodyAnimation;
    private IObjectPool<GameObject> m_BulletPool;
    public IObjectPool<GameObject> BulletPool
    {
        get
        {
            m_BulletPool ??= new ObjectPool<GameObject>(
                    // On Create Pool
                    () =>
                    {
                        var go = Instantiate(m_BulletPrefab);

                        // pool
                        var itemPool = go.GetComponent<IPoolableItem>();
                        itemPool.Pool = BulletPool;
                        return go;

                    },
                    // On Get Pool
                    (GameObject go) =>
                    {
                        Quaternion parentRot = gameObject.transform.parent.transform.rotation;
                        go.transform.position = transform.position;
                        go.transform.rotation = parentRot;
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
            return m_BulletPool;
        }
    }
    private void OnDestroy()
    {
        BulletPool.Clear();
    }

    private void Start()
    {
        shootSpeed = GameManager.Instance.shootSpeed;
        multiplier = GameManager.Instance.Multiplier;
    }

    private void Update()
    {
        //StartCoroutine(Duar());
        if (isAttacking == false)
        {
            StartCoroutine(ShootBullet());
        }
    }
    
    IEnumerator ShootBullet()
    {
        var newSpd = shootSpeed + GameManager.Instance.Multiplier * 0.2f;
        isAttacking = true;
        riffleAnimation.SetBool("isShoot", true);
        riffleAnimation.speed = newSpd;
        //bodyAnimation.SetBool("isShoot", true);

        yield return new WaitForSeconds(1 / newSpd);
        BulletPool.Get();
        //// Instantiate bullet on the top of the launcher
        //Instantiate(m_BulletPrefab, gameObject.transform.position, parentRot);
        GameManager.Instance.PlaySoundFX("TankFire");
        //FindObjectOfType<AudioManager>().PlaySound("TankFire");
        bodyAnimation.SetBool("isShoot", false);
        riffleAnimation.SetBool("isShoot", false);
        isAttacking = false;
    }
}
