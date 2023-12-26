using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public void Play(string audioName)
    {
        AudioManager.Instance.PlaySFX(audioName);
    }
}
