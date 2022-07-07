using UnityEngine;

public class Tembak : MonoBehaviour
{
    private float shootSpeed;
    private float multiplier;
    [SerializeField]
    private GameObject bulletPrefab;

    public Animator riffleAnimation;
    public Animator bodyAnimation;

    private void Start()
    {
        shootSpeed = GameManager.Instance.shootSpeed;
        multiplier = GameManager.Instance.multiplier;
        // Repeat Duar function after 2 seconds delay every 1/shoot_speed seconds
        InvokeRepeating("Duar", 2, 1/shootSpeed); 
    }

    private void Invoker()
    {
        InvokeRepeating("Duar", .1f, 1 / (shootSpeed + multiplier * 0.01f));
    }
    private void Update()
    {
        if (multiplier != GameManager.Instance.multiplier)
        {
            CancelInvoke();
            Invoker();
        }
    }
    private void Duar()
    {
        riffleAnimation.SetBool("isShoot", true);
        riffleAnimation.speed = (shootSpeed + multiplier * 0.01f) + GameManager.Instance.multiplier * 0.005f;
        bodyAnimation.SetBool("isShoot", true);
        
        
        // Get the angle of shooting
        // From its parent's rotation
        Quaternion parentRot = gameObject.transform.parent.transform.rotation;

        // Instantiate bullet on the top of the launcher
        Instantiate(bulletPrefab, gameObject.transform.position, parentRot);
        bodyAnimation.SetBool("isShoot", false);
    }
}