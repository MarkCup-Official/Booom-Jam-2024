using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Box : MonoBehaviour, ICacthable
{
    private Transform owner;
    private Rigidbody2D rb;
    public SpriteRenderer boxlight;
    public float ThrowForce = 3;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        float gb = Mathf.Clamp( Mathf.Abs(Mathf.Sin(Time.time * 4)),0.2f,1f);
        boxlight.color = new Vector4(1f, gb, gb, 1);
    }
    public void OnEnterCatch()
    {
       // rb.isKinematic = true;
        gameObject.SetActive(false);
    }

    public void OnExitCatch()
    {
        gameObject.SetActive(true);
       // rb.isKinematic = false;
        transform.position = owner.transform.position;
        rb.velocity = new Vector2(InputMgr.GetHorizontal(), 1) * ThrowForce;

        //Normalized();
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
       
    }

    public void Set(Transform owner)
    {
        this.owner = owner;
    }
}
public interface ICacthable
{
    public void Set(Transform owner);
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
