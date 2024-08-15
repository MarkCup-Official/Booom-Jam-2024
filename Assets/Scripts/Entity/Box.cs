using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Box : MonoBehaviour
{
    
    private Rigidbody2D rb;
    public SpriteRenderer boxlight;
    public float ThrowForce = 3;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    public void Lock()
    {
        rb.isKinematic = true;
    }
    public void UnLock()
    {
        rb.isKinematic = false;
    }
    private void Update()
    {
        float gb = Mathf.Clamp( Mathf.Abs(Mathf.Sin(Time.time * 4)),0.2f,1f);
        boxlight.color = new Vector4(gb, gb, 1, 1);
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
