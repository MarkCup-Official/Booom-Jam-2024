using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Wind : MonoBehaviour
{
    //public
    public enum Direction
    {
        Up=0, Down=1, Left=2, Right=3,
        Stay=4,
    }

    public Direction direction;
    public float speed;

    public static Vector3 Dir2Vec(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3.up;
            case Direction.Down:
                return Vector3.down;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            case Direction.Stay:
                return Vector3.zero;
        }
        return Vector3.zero;
    }

    public Vector3 GetDirection()
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3.up;
            case Direction.Down:
                return Vector3.down;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            case Direction.Stay:
                return Vector3.zero;
        }
        return Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMgr mgr = collision.GetComponent<PlayerMgr>();
            mgr.moveController.OnFan = true;
            mgr.moveController.OnFanDirection = GetDirection();
            mgr.moveController.OnFanSpeed = speed;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMgr mgr = collision.GetComponent<PlayerMgr>();
            mgr.moveController.OnFan = false;
            mgr.moveController.OnFanDirection = Vector3.zero;
            mgr.moveController.OnFanSpeed = 0;
        }
    }

}
