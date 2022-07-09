using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    public void DestroyAnimation()
    {
        Destroy(gameObject);
    }
}
