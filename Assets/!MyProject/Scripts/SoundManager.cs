using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;
    private void Awake()
    {
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
                audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void PlaySound(AudioClip clip)
    {
            audioSource.PlayOneShot(clip);
    }
}