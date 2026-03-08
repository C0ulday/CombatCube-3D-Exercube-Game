using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHitDetector : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPunchReceived()
    {
        animator.SetTrigger("isHit");
    }

}
