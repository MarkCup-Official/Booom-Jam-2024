using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider2D))]
public class EventTrigger : MonoBehaviour
{
    public Collider2D target;
    public UnityEvent action;
    public bool isOnce = true;
    private bool isDone;
    private BoxCollider2D _collider2D;
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (_collider2D == null) _collider2D = GetComponent<BoxCollider2D>();
        DebugTool.DrawCollider(_collider2D);
        if(target!=null)
        Debug.DrawLine(transform.position, target.transform.position, Color.green);
    }
    private void Awake()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        _collider2D.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target == null) return;
        if (isOnce && isDone) return;
        if (collision.Equals(target))
        {
          
            action?.Invoke();
            isDone = true;
        }
    }
}
