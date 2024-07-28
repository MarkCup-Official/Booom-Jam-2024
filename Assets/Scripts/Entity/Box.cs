using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Box : MonoBehaviour, ICacthable
{
    private Transform owner;
  
    private Vector3 offset;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    public void OnEnterCatch()
    {
      
      
        rb.isKinematic = true;
        offset = transform.position - owner.position;
    }

    public void OnExitCatch()
    {
        
        rb.isKinematic = false;
       
        Normalized();
    }
    /// <summary>
    /// ���λ������
    /// </summary>
    private void Normalized()
    {
        float x = Mathf.Floor(transform.position.x);
        float y = Mathf.Floor(transform.position.y);
        transform.DOMove(new Vector3(x + 0.5f, y + 0.5f, 0), 0.4f);
    }
    
    public void OnUpdate()
    {
        transform.position = owner.transform.position + offset;
    }

    public void SetOwner(Transform owner)
    {
        this.owner = owner;
    }
}
public interface ICacthable
{
    public void SetOwner(Transform owner);
    /// <summary>
    /// ��ץ����ʱִ��
    /// </summary>
    public void OnEnterCatch();
    /// <summary>
    /// ��ץ����ʱÿһִ֡��
    /// </summary>
    public void OnUpdate();
    /// <summary>
    /// ����ץȡʱִ��
    /// </summary>
    public void OnExitCatch();

}