using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotAttack : MonoBehaviour
{

    public Animator animator;

    public float attackInterval =  5f;

    public int damage = 10;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
          
            Attack();
            timer = 0f; // reset
        }


    }

    void Attack()
    {
        animator.SetTrigger("isPunching");
    }



}
