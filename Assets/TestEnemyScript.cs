using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyScript : MonoBehaviour
{
    public float speed = 5f;
    public float changeInterval = 2f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Erste zufällige Richtung
        SetRandomDirection();
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= changeInterval)
        {
            SetRandomDirection();
            timer = 0f;
        }

        // Physikbasierte Bewegung
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);

        CheckBounds();
    }

    void SetRandomDirection()
    {
        moveDirection = new Vector3(
            Random.Range(-1f, 1f),
            0f,
            Random.Range(-1f, 1f)
        ).normalized;
    }

    void CheckBounds()
    {
        Vector3 pos = rb.position;

        bool outOfX = pos.x < -50 || pos.x > 50;
        bool outOfY = pos.y < 100 || pos.y > 200;

        if (outOfX || outOfY)
        {
            rb.position = new Vector3(0, 10, 150);
        }
    }
}

