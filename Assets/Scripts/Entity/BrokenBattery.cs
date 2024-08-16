using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBattery : MonoBehaviour
{

    public GameObject[] lits;
    public GameObject lit;


    float timmer;

    float next;

    bool open = false;

    Rigidbody2D rigidbody2;

    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        next = Random.Range(0.1f, 1);
    }

    private void Update()
    {
        timmer += Time.deltaTime;

        if (timmer > next)
        {
            next = Random.Range(0.1f, 1);
            timmer = 0;
            open=!open;
            lit.SetActive(open);
            foreach (GameObject go in lits)
            {
                if (go != null)
                {
                    go.SetActive(open);
                }
            }
        }
    }


    public void Break()
    {
        foreach (GameObject go in lits)
        {
            lit.SetActive(false);
            if (go != null)
            {
                go.SetActive(true);
            }
        }
        transform.position += Vector3.down;
        rigidbody2.simulated = true;

        enabled = false;
    }
}
