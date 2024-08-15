using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour
{

    public enum Direction
    {
        none,
        clockwise,
        anticlockwise,
    }
    public Direction direction;
    public SelfRotate particleRotate;
    public SelfRotate fanRotate;
    public float speed = 10;
    public float VerticalVelocity = 10;
    public Dictionary<Collider2D, Rigidbody2D> allEnteredObjects = new();

    private void Start()
    {

    }
    private void FixedUpdate()
    {
        if (direction == Direction.none)
        {
            fanRotate.rotateDir = particleRotate.rotateDir = new Vector3(0, 0, 0);
            return;
        }
            

        foreach (var item in allEnteredObjects)
        {
            Vector2 dir = (item.Value.transform.position - transform.position).normalized;
            if (direction == Direction.clockwise)
            {
                Vector2 tangent = new Vector2(dir.y, -dir.x) * speed;
                item.Value.AddForce(tangent);
                item.Value.AddForce(-dir * VerticalVelocity);
                fanRotate.rotateDir =particleRotate.rotateDir = new Vector3(0, 0, -speed * 3);
                Debug.DrawRay(item.Value.transform.position, tangent);
            }
            else if (direction == Direction.anticlockwise)
            {
                Vector2 tangent = new Vector2(-dir.y, dir.x) * speed;
                item.Value.AddForce(tangent);
                item.Value.AddForce(-dir * VerticalVelocity);
                fanRotate.rotateDir = particleRotate.rotateDir = new Vector3(0, 0, speed * 3);
                Debug.DrawRay(item.Value.transform.position, tangent);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
        {
            allEnteredObjects.Add(collision, rb);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (allEnteredObjects.ContainsKey(collision))
        {
            allEnteredObjects.Remove(collision);
        }
    }

}
