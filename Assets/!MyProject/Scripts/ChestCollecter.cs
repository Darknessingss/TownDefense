using Unity.VisualScripting;
using UnityEngine;

public class ChestCollecter : MonoBehaviour
{
    [SerializeField] private BottleBalance bottleBalance;
    [SerializeField] private int BottleRewards = 5;
    [SerializeField] private int MinReward = 2;
    [SerializeField] private int MaxReward = 9;
    [SerializeField] private AudioClip CollectChest;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int randomReward = Random.Range(MaxReward, MinReward);
            bottleBalance.AddBottle(randomReward);
            PlaySoundAtPosition(CollectChest, transform.position);
            Destroy(this.gameObject);
        }
    }
    private void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        GameObject soundGameObject = new GameObject("TempAudio");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(soundGameObject, clip.length);
    }
}
