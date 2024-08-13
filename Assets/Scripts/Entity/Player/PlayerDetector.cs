using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
  
    public Iinteractive  target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactivable"))
        {
            Iinteractive obj;
            if (collision.gameObject.TryGetComponent<Iinteractive>(out obj))
            {
                target = obj;
                obj.OnPlayerEnter(GetComponent<PlayerMgr>());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactivable"))
        {
            Iinteractive obj;
            if (collision.gameObject.TryGetComponent<Iinteractive>(out obj))
            {
                if (obj == target)
                {
                    target = null;
                }

                obj.OnPlayerExit();
            }
        }
    }
    private void Update()
    {
        if (target == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            target.OnPlayerEnterInteractive();
        }
        else if (Input.GetKey(KeyCode.F))
        {
            target.OnPlayerInteractive();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            target.OnPlayerExitInteractive();
        }
    }
}
