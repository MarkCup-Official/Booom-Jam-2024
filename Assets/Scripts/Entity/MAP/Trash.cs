using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public bool DestroyTheCleaner = false;
    public GameObject particle;
    public void OnClean()
    {
        GameObject ptc = ObjectPool.Instance.GetObject(particle);
        ptc.transform.position = transform.position;
    }
}
