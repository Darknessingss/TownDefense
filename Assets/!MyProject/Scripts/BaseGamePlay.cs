using UnityEngine;

public class BaseGamePlay : MonoBehaviour
{
    public int maxHealth = 1500;
    public int currentHealth;
    public MonoBehaviour PlayerMovement;
    public AudioClip loseSound;

    [SerializeField] private GameObject LoseScreen;

    void Start()
    {
        currentHealth = maxHealth;
        if (LoseScreen != null)
        {
            LoseScreen.SetActive(false);
        }
        Time.timeScale = 1;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameObject soundObject = new GameObject("LoseSoundObject");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        if (loseSound != null)
        {
            audioSource.PlayOneShot(loseSound);
        }

        Destroy(soundObject, loseSound != null ? loseSound.length : 2f);

        if (LoseScreen != null)
        {
            LoseScreen.SetActive(true);
            PlayerMovement.enabled = false;
        }
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyMovement enemy = collision.gameObject.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            enemy.TakeDamage(300);
        }
    }
}