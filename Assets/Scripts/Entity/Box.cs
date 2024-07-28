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
    /// 规格化位置坐标
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
    /// 刚抓起来时执行
    /// </summary>
    public void OnEnterCatch();
    /// <summary>
    /// 被抓起来时每一帧执行
    /// </summary>
    public void OnUpdate();
    /// <summary>
    /// 不再抓取时执行
    /// </summary>
    public void OnExitCatch();

}