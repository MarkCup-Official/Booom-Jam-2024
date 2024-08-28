using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerMover : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector3 direction = Vector3.zero;

    private void FixedUpdate()
    {
        if(direction == Vector3.zero)
        {
            return;
        }
        rb.AddForce(direction * speed);
        direction = Vector3.zero;
    }

    public void Up()
    {
        if (enabled)
            direction.y = 1;
    }
    public void Up2(int i)
    {
        if (enabled)
            direction.y = i;
    }
    public void Down()
    {
        if (enabled)
            direction.y = -1;
    }
    public void Left()
    {
        if (enabled)
            direction.x = -1;
    }
    public void Right()
    {
        if (enabled)
            direction.x = 1;
    }
}
