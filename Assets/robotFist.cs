using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotFist : MonoBehaviour
{

    public int damage = 10; // damage to the player


    private void OnTriggerEnter(Collider other)
    {
        // look if it's the player
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
