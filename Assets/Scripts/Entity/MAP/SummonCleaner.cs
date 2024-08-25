using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCleaner : MonoBehaviour
{
    public Trash[] trashes;

    public void Summon()
    {
        foreach (var tr in trashes)
        {
            tr.gameObject.SetActive(true);
        }

        GameObject go=Instantiate(gameObject);

        go.GetComponent<Cleaner>().enabled = true;
    }
}
