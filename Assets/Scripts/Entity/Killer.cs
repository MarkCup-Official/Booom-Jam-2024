using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerMgr mgr))
            {
                mgr.Kill();
            }
        }
    }

    public void On()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0,0.63f,1);
    }
    public void Off()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
