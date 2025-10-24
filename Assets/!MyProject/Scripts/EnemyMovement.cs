using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public float currentHealth;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float stopDistance = 2f;

    private Rigidbody rb;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        FindTarget();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        MoveTowardsTarget();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Warrior"))
        {
            Debug.Log("Столкновение с воином!");
            Destroy(collision.gameObject);

            target = null;
            FindTarget();

            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void FindTarget()
    {
        GameObject[] warriors = GameObject.FindGameObjectsWithTag("Warrior");
        if (warriors.Length == 0)
        {
            target = null;
            return;
        }

        float minDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject warrior in warriors)
        {
            float dist = Vector3.Distance(transform.position, warrior.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = warrior.transform;
            }
        }

        target = nearest;
    }

    void MoveTowardsTarget()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= stopDistance)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;

        Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        Vector3 movement = transform.forward * moveSpeed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        if (target == null || Time.frameCount % 120 == 0)
        {
            FindTarget();
        }
    }
}
