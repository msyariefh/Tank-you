using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator backgroundAnimation;
    public Animator skullsAnimation;

    public float transitionTime = 1f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        skullsAnimation.SetBool("isStarting", true);
        backgroundAnimation.SetBool("isStarting", true);
        //SceneManager.LoadSceneAsync(1);
        yield return new WaitForSeconds(transitionTime);
        backgroundAnimation.SetBool("isStarting", false);
        skullsAnimation.SetBool("isStarting", false);
        SceneManager.LoadScene(levelIndex);
        Time.timeScale = 1f;
        MenuManager.Instance.state = MenuManager.State.Gameplay;
    }
}
