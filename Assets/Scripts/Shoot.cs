using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private float shootSpeed;
    private float multiplier;
    private bool isAttacking = false;
    private bool isInWait = true;

    [SerializeField] private GameObject bulletPrefab;
    private AudioSource sfx;
    public Animator riffleAnimation;
    public Animator bodyAnimation;

    private void Start()
    {
        shootSpeed = GameManager.Instance.shootSpeed;
        multiplier = GameManager.Instance.multiplier;
        sfx = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //StartCoroutine(Duar());
        if (isInWait == true)
        {
            isInWait = false;
            Invoke("Wait", 1f);
            return;
        }
        if (isAttacking == false)
        {
            StartCoroutine(Duar());
        }
    }
    
    IEnumerator Duar()
    {
        isAttacking = true;
        riffleAnimation.SetBool("isShoot", true);
        riffleAnimation.speed = (shootSpeed + multiplier * 0.01f) + GameManager.Instance.multiplier * 0.025f;
        //bodyAnimation.SetBool("isShoot", true);

        yield return new WaitForSeconds(1 / (shootSpeed + multiplier * 0.05f));
        Quaternion parentRot = gameObject.transform.parent.transform.rotation;

        // Instantiate bullet on the top of the launcher
        Instantiate(bulletPrefab, gameObject.transform.position, parentRot);
        sfx.Play();
        bodyAnimation.SetBool("isShoot", false);
        riffleAnimation.SetBool("isShoot", false);
        isAttacking = false;
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }
}
