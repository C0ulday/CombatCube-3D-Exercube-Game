using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationScriptController : MonoBehaviour
{
    Animator animator;

    void Start() => animator = GetComponent<Animator>();

    void Update()
    {
        bool isPressingW = Input.GetKey(KeyCode.W);
        animator.SetBool("isPunching", isPressingW);
    }
}
