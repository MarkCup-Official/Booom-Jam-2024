using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentKiller : MonoBehaviour
{
    Transform p;
    void Start()
    {
        p = transform.parent;
    }

    void Update()
    {
        if (p.position.y > transform.position.y)
        {
            transform.SetParent(null);
        }
        else
        {
            transform.SetParent(p);
        }
    }
}
