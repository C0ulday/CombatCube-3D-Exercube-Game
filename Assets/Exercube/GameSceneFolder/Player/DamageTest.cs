using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public int health;
    // Start is called before the first frame update
    public void takeDamage() 
    {
        health = GetComponent<EnemyScript>().health;
        health -= 20;
        GetComponent<EnemyScript>().health = health;  
        if (health <= 0) { die(); }
    }

    private void die() { UnityEngine.Debug.Log("tot"); }
}
