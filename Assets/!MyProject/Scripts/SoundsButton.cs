using UnityEngine;
using UnityEngine.UI;

public class SoundsButton : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        // Используем SoundManager вместо PlayClipAtPoint
        if (SoundManager.Instance != null && clickSound != null)
        {
            SoundManager.Instance.PlaySound(clickSound);
        }
    }
}