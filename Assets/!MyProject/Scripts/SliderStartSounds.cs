using UnityEngine;
using UnityEngine.UI;

public class SliderStartSounds : MonoBehaviour
{
    [Header("Настройки звука")]
    public AudioClip SoundSlider;
    public float soundCooldown = 0.5f;

    private Slider slider;
    private AudioSource audioSource;
    private float lastSoundTime;
    private bool SoundAttack = false;

    void Start()
    {
        slider = GetComponent<Slider>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        if (value <= 0f && !SoundAttack)
        {
            if (Time.time - lastSoundTime >= soundCooldown)
            {
                PlaySoundSlider();
                lastSoundTime = Time.time;
            }
            SoundAttack = true;
        }
        else if (value > 0f)
        {
            SoundAttack = false;
        }
    }

    void PlaySoundSlider()
    {
        if (SoundSlider != null && audioSource != null)
        {
            audioSource.PlayOneShot(SoundSlider);
        }
    }

    public void PlaySoundSliderAttack()
    {
        PlaySoundSlider();
    }
}
