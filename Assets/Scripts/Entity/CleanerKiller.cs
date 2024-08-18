using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerKiller : MonoBehaviour
{
    public Cleaner cleaner;

    public GameObject particle;

    private void Awake()
    {
        cleaner = GetComponent<Cleaner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if (collision.CompareTag("Killer"))
            {
                GameObject ptc = ObjectPool.Instance.GetObject(particle);
                ptc.transform.position = transform.position;
                cleaner.ResetPos();
                ptc = ObjectPool.Instance.GetObject(particle);
                ptc.transform.position = transform.position;
            }
        }
    }
}
