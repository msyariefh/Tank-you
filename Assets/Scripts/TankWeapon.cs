using UnityEngine;
using UnityEngine.Pool;

public class TankWeapon : MonoBehaviour, IPoolableItem
{
    public ParticleSystem boom;

    public IObjectPool<GameObject> Pool { get; set; }
    private void OnDisable()
    {
        Pool.Release(gameObject);
    }
    private float additionalSpeed;
    private void OnEnable()
    {
        additionalSpeed = GameManager.Instance.Multiplier * .01f;
    }

    // This is Bullet properties
    private void Update()
    {
        transform.Translate(0, (1f + additionalSpeed) *
           GameManager.Instance.ShootVelocity *
           Time.deltaTime * 30f, 0);
        // Move towards Y axis
        //transform.Translate(0, (GameManager.Instance.ShootVelocity + 
        //    (GameManager.Instance.Multiplier * 0.01f)) * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        // If exeeds the world borders, then destroy
        if (trigger.gameObject.CompareTag("World Border"))
        {
            gameObject.SetActive(false);
            return;
        }

        if (trigger.gameObject.CompareTag("Barier"))
        {
            gameObject.SetActive(false);
            return;
        }

        if (trigger.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.AddScores(1);
            Instantiate(boom, transform.position, Quaternion.identity);
            GameManager.Instance.PlaySoundFX("AlienExplode");
            //FindObjectOfType<AudioManager>().PlaySound("AlienExplode");
            //Destroy(trigger.gameObject);
            //Destroy(this.gameObject);
            trigger.gameObject.SetActive(false);
            gameObject.SetActive(false);
            return;
        }
    }
}
