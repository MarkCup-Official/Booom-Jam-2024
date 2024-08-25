using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    bool on = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!on)
        {
            return;
        }
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
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0.3742138f, 0.3742138f);
        on = true;
    }
    public void Off()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
        on = false;
    }
}
