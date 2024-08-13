using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    public Vector3 rotateDir;

    private void Update()
    {
        transform.Rotate(rotateDir * Time.deltaTime);
    }
}
