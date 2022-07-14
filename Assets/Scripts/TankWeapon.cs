using UnityEngine;

public class TankWeapon : MonoBehaviour
{
    public ParticleSystem boom;
    // This is Bullet properties
    private void Update()
    {
        // Move towards Y axis
        transform.Translate(0, (GameManager.Instance.shootVelocity + 
            (GameManager.Instance.multiplier * 0.01f)) * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        // If exeeds the world borders, then destroy
        if (trigger.gameObject.CompareTag("World Border"))
        {
            Destroy(this.gameObject);
            return;
        }

        if (trigger.gameObject.CompareTag("Barier"))
        {
            Destroy(this.gameObject);
            return;
        }

        if (trigger.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.AddScores(1);
            Instantiate(boom, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().PlaySound("AlienExplode");
            Destroy(trigger.gameObject);
            Destroy(this.gameObject);
            return;
        }
    }
}
