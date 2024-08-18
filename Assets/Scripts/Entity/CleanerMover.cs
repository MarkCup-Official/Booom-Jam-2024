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

    public void Up()
    {
        rb.AddForce ( Vector3.up * speed);
    }
    public void Down()
    {
        rb.AddForce(Vector3.down * speed);
    }
    public void Left()
    {
        rb.AddForce(Vector3.left * speed);
    }
    public void Right()
    {
        rb.AddForce(Vector3.right * speed);
    }
}
