using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cleaner : MonoBehaviour
{

    private bool isRayActive = true;
    private Rigidbody2D rb;
    public Transform StartPos;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Check();
    }
    public void Check()
    {
        CheckSingleDir(Vector2.up);
        CheckSingleDir(Vector2.right);
        CheckSingleDir(Vector2.down);
        CheckSingleDir(Vector2.left) ;
    }
    public void ResetPos()
    {
        if (StartPos == null) return;

        transform.position = StartPos.transform.position;

    }
    public void CheckSingleDir(Vector2 dir)
    {
        if (!isRayActive)
            return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position,dir, 1, 1 << 7 | 1<<13);
        if (hit)
            if (hit.collider.CompareTag("Trash"))
            {
                if (hit.collider.TryGetComponent(out Trash trash))
                {
                    hit.collider.gameObject.SetActive(false);
                    transform.DOMove(hit.collider.transform.position, 0.1f);
                    isRayActive = false;
                    rb.isKinematic = true;
                    TimeDelay.Instance.Delay(0.1f, () =>
                    {
                        isRayActive = true;
                        rb.isKinematic = false;
                    });
                }

            }
    }
}
