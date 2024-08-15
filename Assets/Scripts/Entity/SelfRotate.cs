using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    public Vector3 rotateDir;
    public float speed = 1;
    private void Update()
    {
        transform.Rotate(rotateDir * Time.deltaTime * speed);
    }
}
