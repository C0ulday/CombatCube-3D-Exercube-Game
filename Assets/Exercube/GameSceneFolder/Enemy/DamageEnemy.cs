using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public void takeDamage()
    {
        // Update the Health of the Enemy -Script
        GetComponent<Enemy>().EnemyHealth = GetComponent<Enemy>().EnemyHealth - 20;
        
    }
}
