using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private PlayerMgr mgr;
    public float TargetAddSize;
    public float lerpValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out mgr))
            {
                mgr.targetCamera.target = transform;
                mgr.targetCamera.lerpTarget = lerpValue;
                mgr.targetCamera.addSize = TargetAddSize;
                mgr.targetCamera.LerpMultiplier = 0;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mgr.targetCamera.lerpTarget = 0;
            mgr.targetCamera.addSize = 0;
            mgr.targetCamera.LerpMultiplier = 0;
        }
    }
}
