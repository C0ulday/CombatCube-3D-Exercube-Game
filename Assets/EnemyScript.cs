using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public Collider groundCollider;
    public float margin = 1f;
    public int health;
    //private int count;
    private float leftX;
    private float rightX;
    private int direction = 1;
    //Healthbar
    public Healthbar healthbar;


    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
        health = 100;
        //iniziieren der healthbar
        healthbar.SetMaxHealth(health);
        //count = 0;
        rb = GetComponent<Rigidbody>();
        Bounds b = groundCollider.bounds;

        leftX = b.min.x + margin;
        rightX = b.max.x - margin;

    }

    // Update is called once per frame
    void Update()
    {
        //healthbar update -- FALLS ERROR: wahrscheinlich health < 0
        healthbar.SetHealth(health);
        if (health <= 0) { return; }
        
        // Bewegung horizontal
        rb.MovePosition(rb.position + new Vector3(direction * speed * Time.fixedDeltaTime, 0, 0));

        // Grenzen prüfen
        if (rb.position.x > rightX)
            direction = -1;

        if (rb.position.x < leftX)
            direction = 1;
    }
}
