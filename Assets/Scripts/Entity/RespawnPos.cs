using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RespawnPos : MonoBehaviour
{
    private BoxCollider2D _collider2D;
    public bool isDone;
    private void Awake()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        _collider2D.isTrigger = true;
    }
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (_collider2D == null) _collider2D = GetComponent<BoxCollider2D>();
        DebugTool.DrawCollider(_collider2D);
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDone) return;
        if (collision.CompareTag("Player"))
        {
            if(collision.gameObject.TryGetComponent(out PlayerMgr mgr))
            {
                mgr.SetRespawnPos(transform);
            }
            isDone = true;
        }
    }
}
