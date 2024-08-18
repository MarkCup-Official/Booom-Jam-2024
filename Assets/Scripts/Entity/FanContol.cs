using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanContol : MonoBehaviour
{
    public GameObject[] selfRotateGo;
    public SelfRotate[] selfRotate;

    private void Start()
    {
        selfRotate = new SelfRotate[selfRotateGo.Length];

        for ( int i = 0; i < selfRotateGo.Length; i++ )
        {
            selfRotate[i] = selfRotateGo[i].transform.Find("object_23").GetComponent<SelfRotate>();
        }
    }

    public void Open()
    {
        foreach (var t in selfRotate)
        {
            t.enabled = true;
        }
    }

    public void Close()
    {
        foreach (var t in selfRotate)
        {
            t.enabled = false;
        }
    }
}