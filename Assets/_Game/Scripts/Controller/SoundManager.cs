using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip betClip, spinClip, coinClip;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SoundBet()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(betClip);
    }

    public void SoundSpin()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(spinClip);
    }

    public void SoundCoin()
    {
        /*audioSource.pitch = 3f;
        audioSource.PlayOneShot(coinClip);*/
    }

    public void StopSount()
    {
        audioSource.Stop();
    }
}