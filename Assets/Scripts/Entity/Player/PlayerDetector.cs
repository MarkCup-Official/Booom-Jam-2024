using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
  
    public List<Iinteractive>  target=new List<Iinteractive>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactivable"))
        {
            Iinteractive obj;
            if (collision.gameObject.TryGetComponent<Iinteractive>(out obj))
            {
                target.Add(obj);
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
                
                target.Remove(obj);

                obj.OnPlayerExit();
            }
        }
    }
    private void Update()
    {
        if (target.Count==0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach(Iinteractive iinteractive in target)
                iinteractive.OnPlayerEnterInteractive();
        }
        else if (Input.GetKey(KeyCode.F))
        {
            foreach (Iinteractive iinteractive in target)
                iinteractive.OnPlayerInteractive();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            foreach (Iinteractive iinteractive in target)
                iinteractive.OnPlayerExitInteractive();
        }
    }
}
