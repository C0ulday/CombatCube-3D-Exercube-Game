using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnBossOnX : MonoBehaviour
{
    public UnityEvent onXPressed;
    public UnityEvent onIPressed;
    public UnityEvent onOPressed;
    public UnityEvent onKPressed;
    public UnityEvent onLPressed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            onXPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            onIPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            onOPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            onKPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            onLPressed?.Invoke();
        }
    }
}

