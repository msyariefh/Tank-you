using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
        AudioManager.Instance.PlayBgmSound("Arena");
        //FindObjectOfType<AudioManager>().PlaySound("MainMenu", "Arena");
    }

    public void DestroyAnimation()
    {
        Destroy(gameObject);
    }
}
