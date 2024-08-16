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
