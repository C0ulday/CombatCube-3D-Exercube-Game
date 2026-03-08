using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("Rigidbody gefunden: " + rb);
    }

    void FixedUpdate()
    {
        Vector3 dir = Vector3.forward;  // konstant nach vorne
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }
}

