using UnityEngine;

public class BaseGamePlay : MonoBehaviour
{
    public int maxHealth = 1500;
    public int currentHealth;
    public MonoBehaviour PlayerMovement;

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
        Destroy(gameObject);
        if (gameObject != null)
        {
            LoseScreen.SetActive(true);
            PlayerMovement.enabled = false;
        }
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
