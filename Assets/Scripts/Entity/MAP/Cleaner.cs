using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cleaner : MonoBehaviour
{

    private bool isRayActive = true;
    private Rigidbody2D rb;
    public Transform StartPos;
    private AudioSource audioSource;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
        gameObject.SetActive(true);
        if (StartPos == null) return;

        transform.position = StartPos.transform.position;

        isRayActive = true;
        rb.isKinematic = false;

    }
    public void CheckSingleDir(Vector2 dir)
    {
        if (!isRayActive)
            return;

        const float time = 0.3f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,dir, 1.4f, 1 << 7 |1<<14);
        if (hits.Length>0)
            foreach(var hit in hits)
                if (hit.collider.CompareTag("Trash"))
                {
                    if (hit.collider.TryGetComponent(out Trash trash))
                    {
                        rb.velocity = Vector2.zero;
                        hit.collider.gameObject.SetActive(false);
                        isRayActive = false;
                        rb.isKinematic = true;
                        audioSource.PlayOneShot(SoundManager.Instance.GetClip("piaji"));

                        trash.OnClean();
                        transform.DOMove(hit.collider.transform.position, time - 0.1f);
                        if (trash.DestroyTheCleaner == true)
                        {
                            gameObject.SetActive(false);
                            return;
                        }


                        TimeDelay.Instance.Delay(time, () =>
                        {
                            isRayActive = true;
                            rb.isKinematic = false;
                        });
                    }

                }
    }
}
