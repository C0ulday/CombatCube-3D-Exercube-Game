using UnityEngine;

public class EnemyHitDetector : MonoBehaviour
{
    private Enemy enemy;

    private void Start()
    {
        // Get the Enemy component on the same GameObject
        enemy = GetComponent<Enemy>();
        
        if (enemy == null)
        {
            Debug.LogError("[EnemyHitDetector] No Enemy component found on this GameObject!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[EnemyHitDetector] Trigger entered by: " + other.name + " (Tag: " + other.tag + ")");

        // Only react to objects tagged as "Hand"
        if (!other.CompareTag("Hand"))
        {
            Debug.Log("[EnemyHitDetector] Not a hand → ignored.");
            return;
        }

        Debug.Log("[EnemyHitDetector] Hit by HAND → Enemy will die!");

        // Kill the enemy immediately
        enemy?.Die();
    }
}
