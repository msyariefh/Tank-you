using UnityEngine;

public class Tank : MonoBehaviour
{
    private GameObject tankWeapon;
    private bool moveRight = false;
    private bool moveLeft = false;
    private float limit;
    private bool activateHelper;
    [SerializeField]
    private GameObject helper;
    //private Transform pos;

    private void Start()
    {
        tankWeapon = gameObject.transform.GetChild(0).gameObject;
        limit = GameManager.Instance.maximumShootAngle;
        activateHelper = GameManager.Instance.activateHelper;

        helper.SetActive(activateHelper);
    }
    private void FixedUpdate()
    {
        if (moveLeft)
        {
            ToTheLeft();
        }
        if (moveRight)
        {
            ToTheRight();
        }

    }

    private void ToTheLeft()
    {
        if (tankWeapon.transform.rotation.z >= limit) { return; }
        tankWeapon.transform.Rotate(Vector3.forward, GameManager.Instance.sensitivity );
    }

    private void ToTheRight()
    {
        if (tankWeapon.transform.rotation.z <= -limit) { return; }
        tankWeapon.transform.Rotate(Vector3.forward, GameManager.Instance.sensitivity * (-1));
    }

    // Controller
    public void PressLeft()
    {
        moveLeft = true;
        //ToTheLeft();
    }
    public void PressRight() 
    {
        moveRight = true;
        //ToTheRight();
    }
    public void UnPressLeft()
    {
        moveLeft = false;
    }
    public void UnPressRight()
    {
        moveRight = false;
    }


}
